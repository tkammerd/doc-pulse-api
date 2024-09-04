namespace Ots.AppDmDoc.Abstractions.HashIds;

public interface IHashIdsAdaptor
{
    // Single Encoder
    int DecodeSingle(string value);
    string Encode(int value);
    IHashIdsAdaptor InitEncoder(string saltPrefix);


    // Multiple-Encoders
    bool IsInitialized<T>();
    bool IsInitialized(Type entityType);

    bool TryInitEncoder<T>();
    bool TryInitEncoder(Type entityType);

    string Encode(Type entityType, int value);
    int DecodeSingle(Type entityType, string value);

    string Encode(string entitySalt, int value);
    int DecodeSingle(string entitySalt, string value);

    int DecodeIdentifier(string hashId);

    int? TryDecodeIdentifier(string? hashId);
}

public interface IHashIdsAdaptor<ControllerType> : IHashIdsAdaptor
{
}