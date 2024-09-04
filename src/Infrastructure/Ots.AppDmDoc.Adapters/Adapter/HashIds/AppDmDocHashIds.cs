using HashidsNet;
using Microsoft.Extensions.Options;
using Ots.AppDmDoc.Abstractions.HashIds;

namespace Ots.AppDmDoc.Adapters.Adapter.HashIds;

public class HashIdsAdaptor<TEntity> : IHashIdsAdaptor<TEntity>
{
    protected readonly HashIdsAdaptorConfig? _config;

    ////Dictionary<Type, Hashids> HashEncoders = new Dictionary<Type, Hashids>();
    private Hashids Encoder { get; set; }

    public HashIdsAdaptor(IOptions<HashIdsAdaptorConfig> options) : this(options.Value.Salt, options.Value.MinHashLength)
    {
        _config = options.Value;
    }

    public HashIdsAdaptor(string salt, int minHashLength)
    {
        Encoder = new($"{nameof(TEntity)}-{salt}", minHashLength);
    }

    ////public Hashids GetEncoder { get; set; }

    public string Encode(int value) => Encoder.Encode(value);
    public int DecodeSingle(string value) => Encoder.DecodeSingle(value);

    public int DecodeIdentifier(string hashId)
    {
        if (Encoder.TryDecodeSingle(hashId, out var id))
            return id;

        //throw new MediatRException($"The Identifier Sepcified [{hashId}] is NOT Valid, but if at First You Don't Succeed...");
        throw new Exception($"The Identifier Sepcified [{hashId}] is NOT Valid, but if at First You Don't Succeed...");
    }

    public int? TryDecodeIdentifier(string? hashId)
    {
        if (string.IsNullOrEmpty(hashId))
            return null;

        if (Encoder.TryDecodeSingle(hashId, out var id))
            return id;

        return null;
    }


    public bool IsInitialized => Encoder != null;
    public IHashIdsAdaptor InitEncoder(string saltPrefix) => throw new NotImplementedException();



    bool IHashIdsAdaptor.IsInitialized<T>() => throw new NotImplementedException("Not Implemented for Single Encoder");
    bool IHashIdsAdaptor.IsInitialized(Type entityType) => throw new NotImplementedException("Not Implemented for Single Encoder");

    public bool TryInitEncoder<T>() => throw new NotImplementedException("Not Implemented for Single Encoder");
    public bool TryInitEncoder(Type entityType) => throw new NotImplementedException("Not Implemented for Single Encoder");

    public string Encode(Type entityType, int value) => throw new NotImplementedException("Not Implemented for Single Encoder");
    public int DecodeSingle(Type entityType, string value) => throw new NotImplementedException("Not Implemented for Single Encoder");

    public string Encode(string entitySalt, int value) => throw new NotImplementedException("Not Implemented for Single Encoder");
    public int DecodeSingle(string entitySalt, string value) => throw new NotImplementedException("Not Implemented for Single Encoder");
}

public class HashIdsAdaptor : IHashIdsAdaptor
{
    private Dictionary<string, Hashids> Encoders { get; set; } = new();


    protected readonly HashIdsAdaptorConfig _config;

    ////Dictionary<Type, Hashids> HashEncoders = new Dictionary<Type, Hashids>();
    private Hashids? Encoder { get; set; }

    public HashIdsAdaptor(IOptions<HashIdsAdaptorConfig> options) //: this(options.Value.Salt, options.Value.MinHashLength)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        _config = options.Value;
    }

    //public HashIdsAdaptor(string salt, int minHashLength)
    //{
    //    Encoder = new($"{typeof(ControllerType).Name}-{salt}", minHashLength);
    //}

    public bool IsInitialized<T>() => Encoders[typeof(T).Name] != null;
    public bool IsInitialized(Type entityType) => Encoders[entityType.GetType().Name] != null;

    public bool TryInitEncoder<T>() => TryInitEncoder(typeof(T));
    public bool TryInitEncoder(Type entityType)
    {
        Encoders.TryGetValue(entityType.Name, out var encoder);

        if (encoder == null)
        {
            string saltPrefix = entityType.Name;

            Encoders.Add(saltPrefix, new($"{saltPrefix}-{_config.Salt}", _config.MinHashLength));

            return true;
        }

        return false;
    }
    public bool TryInitEncoder(string entitySalt)
    {
        Encoders.TryGetValue(entitySalt, out var encoder);

        if (encoder == null)
        {
            Encoders.Add(entitySalt, new($"{entitySalt}-{_config.Salt}", _config.MinHashLength));

            return true;
        }

        return false;
    }

    ////public Hashids GetEncoder { get; set; }

    public string Encode(Type entityType, int value)
    {
        TryInitEncoder(entityType);

        return Encoders[entityType.Name]?.Encode(value) ?? throw new Exception("Uninitialized Encoder - Base HashIdsAdaptor msut be initialized with a salt-prefix");
    }
    public int DecodeSingle(Type entityType, string value)
    {
        TryInitEncoder(entityType);

        return Encoders[entityType.Name]?.DecodeSingle(value) ?? throw new Exception("Uninitialized Encoder - Base HashIdsAdaptor msut be initialized with a salt-prefix");
    }

    public string Encode(string entitySalt, int value)
    {
        TryInitEncoder(entitySalt);

        return Encoders[entitySalt]?.Encode(value) ?? throw new Exception("Uninitialized Encoder - Base HashIdsAdaptor msut be initialized with a salt-prefix");
    }
    public int DecodeSingle(string entitySalt, string value)
    {
        TryInitEncoder(entitySalt);

        return Encoders[entitySalt]?.DecodeSingle(value) ?? throw new Exception("Uninitialized Encoder - Base HashIdsAdaptor msut be initialized with a salt-prefix");
    }

    public string Encode(int value) => Encoder?.Encode(value) ?? throw new Exception("Uninitialized Encoder - Base HashIdsAdaptor msut be initialized with a salt-prefix");
    public int DecodeSingle(string value) => Encoder?.DecodeSingle(value) ?? throw new Exception("Uninitialized Encoder - Base HashIdsAdaptor msut be initialized with a salt-prefix");

    public IHashIdsAdaptor InitEncoder(string saltPrefix)
    {
        Encoder = new($"{saltPrefix}-{_config.Salt}", _config.MinHashLength);

        return this;
    }

    public int DecodeIdentifier(string hashId) => throw new NotImplementedException();
    public int? TryDecodeIdentifier(string? hashId) => throw new NotImplementedException();
}
