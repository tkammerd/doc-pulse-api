using AutoMapper;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities._Kernel;
using Ots.AppDmDoc.Abstractions.HashIds;

namespace Doc.Pulse.Infrastructure.Resolvers;

public class EncodeHashIdResolver<TEntity> : IMemberValueResolver<EntityBase<int>, object, int, string?>
{
    private readonly IHashIdsAdaptor<TEntity> _hashIds;

    public EncodeHashIdResolver(IHashIdsAdaptor<TEntity> hashIds)
    {
        _hashIds = hashIds;
    }

    public string? Resolve(EntityBase<int> source, object destination, int srcMemeber, string? destMember, ResolutionContext context)
    {
        return _hashIds.Encode(srcMemeber);
    }
}

public class EncodeNullableHashIdResolver<TEntity> : IMemberValueResolver<EntityBase<int>, object, int?, string?>
{
    private readonly IHashIdsAdaptor<TEntity> _hashIds;

    public EncodeNullableHashIdResolver(IHashIdsAdaptor<TEntity> hashIds)
    {
        _hashIds = hashIds;
    }

    public string? Resolve(EntityBase<int> source, object destination, int? srcMemeber, string? destMember, ResolutionContext context)
    {
        if (srcMemeber == null)
            return null;

        return _hashIds.Encode(srcMemeber.Value);
    }
}

public class DecodeHashIdResolver<TEntity> : IMemberValueResolver<object, EntityBase<int>, string, int>
{
    private readonly IHashIdsAdaptor<TEntity> _hashIds;

    public DecodeHashIdResolver(IHashIdsAdaptor<TEntity> hashIds)
    {
        _hashIds = hashIds;
    }

    public int Resolve(object source, EntityBase<int> destination, string srcMember, int destMember, ResolutionContext context)
    {
        return _hashIds.DecodeSingle(srcMember);
    }
}

public class DecodeRequestHashIdResolver<TEntity> : IMemberValueResolver<object, object, string?, int>
{
    private readonly IHashIdsAdaptor<TEntity> _hashIds;

    public DecodeRequestHashIdResolver(IHashIdsAdaptor<TEntity> hashIds)
    {
        _hashIds = hashIds;
    }

    public int Resolve(object source, object destination, string? srcMember, int destMember, ResolutionContext context)
    {
        if (srcMember == null)
            throw new ArgumentNullException(nameof(srcMember), "Unable to Resolve Entity Id Durning Mapping - NULL HashId Specified in Query.");

        //var id = _hashIds.TryDecodeIdentifier(srcMember);

        //if (id == null)
        //    throw new Exception("Decoding Issue");
        //else
        //    return id.Value;


        return _hashIds.DecodeSingle(srcMember);
    }
}


public class EncodeHashIdProjectionResolver : IMemberValueResolver<BaseEntityProjection<int>, object, int, string>
{
    private readonly IHashIdsAdaptor _hashIds;

    public EncodeHashIdProjectionResolver(IHashIdsAdaptor hashIds)
    {
        _hashIds = hashIds;
    }

    public string Resolve(BaseEntityProjection<int> source, object destination, int srcMemeber, string destMember, ResolutionContext context)
    {
        return _hashIds.Encode(source.EntityType, srcMemeber);
    }
}
