using AutoMapper;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class DirtyMappingExtensions
{
    public static IMappingExpression<TSource, TDestination> MapDirtyOnly<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
    {
        map.ForAllMembers(source =>
        {
            source.Condition((sourceObj, destObj, sourceProperty, destProperty) =>
            {
                return !sourceProperty?.Equals(destProperty) ?? !(destProperty == null);
            });
        });

        return map;
    }

    public static IMappingExpression<TSource, TDestination> MapDirtyPropertiesOnlyAndIgnoreRelated<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
    {
        map.ForAllMembers(source =>
        {
            source.Condition((sourceObj, destObj, sourceProperty, destProperty, resolutionContext) =>
            {
                var isNotZeroIdForFk = !(source.DestinationMember.Name.EndsWith("Id") && (sourceProperty is int @int && @int == 0 || sourceProperty is Guid @Guid && @Guid == Guid.Empty || sourceProperty is string @string && string.IsNullOrEmpty(@string)));

                var isDirty = !sourceProperty?.Equals(destProperty) ?? !(destProperty == null);
                var isNotPrimaryIdField = !source.DestinationMember.Name.Equals("Id");
                var isNotNullSourceValue = sourceProperty != null;

                var isNotRelatedObject = sourceProperty is string || sourceProperty is Guid || !(sourceProperty?.GetType().IsClass ?? true);

                return isDirty && isNotPrimaryIdField && isNotZeroIdForFk && isNotNullSourceValue && isNotRelatedObject;
            });
        });

        return map;
    }
}

