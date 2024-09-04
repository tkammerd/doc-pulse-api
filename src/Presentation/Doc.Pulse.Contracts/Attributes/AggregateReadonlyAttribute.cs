namespace Doc.Pulse.Contracts.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class AggregateReadonlyAttribute : Attribute
{
    private readonly bool _hidden;
    private readonly bool _readOnly;

    public bool IsHidden => _hidden;
    public bool IsReadOnly => _readOnly;

    public AggregateReadonlyAttribute(bool hidden = false, bool readOnly = true)
    {
        _hidden = hidden;
        _readOnly = readOnly;
    }
}
