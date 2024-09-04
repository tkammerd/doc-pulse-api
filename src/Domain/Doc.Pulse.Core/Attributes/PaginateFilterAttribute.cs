namespace Doc.Pulse.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PaginateFilterAttribute : Attribute
{
    //private readonly Type _propertyType;

    //public Type PropertyType => _propertyType;

    public PaginateFilterAttribute() //Type propertyType)
    {
        //_propertyType = propertyType;
    }
}
