using AutoMapper;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Ots.AppDmDoc.Adapters.Adapter.Mapper;

public class MapperBaseAdapter : IMapperAdapter
{
    private readonly IMapper _impl;

    public MapperBaseAdapter(IMapper impl)
    {
        _impl = impl;
    }

    public MapperBaseAdapter(MapperConfiguration mapperConfig)
    {
        _impl = mapperConfig.CreateMapper();
    }

    public TDestination Map<TDestination>(object source)
    {
        return _impl.Map<TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return _impl.Map<TSource, TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return _impl.Map(source, destination);
    }

    public object Map(object source, Type sourceType, Type destinationType)
    {
        return _impl.Map(source, sourceType, destinationType);
    }

    public object Map(object source, object destination, Type sourceType, Type destinationType)
    {
        return Map(source, destination, sourceType, destinationType);
    }
}
