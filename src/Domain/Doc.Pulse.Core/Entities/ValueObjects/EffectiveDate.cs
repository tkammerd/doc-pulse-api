using AppDmDoc.SharedKernel.Core.Abstractions;
using Doc.Pulse.Core.Abstractions;

namespace Doc.Pulse.Core.Entities.ValueObjects;

public class EffectiveDate : ValueObject
{
    private readonly TimeProvider? _defaultEffectiveClockProvider;
    private DateTimeOffset? _effective;
    public DateTimeOffset Effective { get { return _effective ?? _defaultEffectiveClockProvider?.GetLocalNow() ?? throw new ArgumentNullException("IDateTimeProvider"); } }

    private EffectiveDate() { }
    private EffectiveDate(DateTimeOffset? effective)
    {
        _effective = effective;
    }
    private EffectiveDate(TimeProvider? defaultEffectiveClockProvider, DateTimeOffset? effective)
    {
        _defaultEffectiveClockProvider = defaultEffectiveClockProvider;
        _effective = effective;
    }

    public static EffectiveDate New(DateTimeOffset defaultValue, DateTimeOffset? effectiveDate = null)
    {
        var effective = new EffectiveDate(effectiveDate ?? defaultValue);

        return effective;
    }

    public static EffectiveDate New(TimeProvider defaultEffectiveClockProvider, DateTimeOffset? effectiveDate = default)
    {
        ArgumentNullException.ThrowIfNull(defaultEffectiveClockProvider);

        //var effective = new EffectiveDate(defaultEffectiveClockProvider, effectiveDate ?? defaultEffectiveClockProvider.Now);
        var effective = new EffectiveDate(defaultEffectiveClockProvider, effectiveDate);

        return effective;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Effective; 
    }
}
