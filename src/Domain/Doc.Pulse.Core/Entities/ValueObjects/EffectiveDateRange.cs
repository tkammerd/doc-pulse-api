using AppDmDoc.SharedKernel.Core.Abstractions;

namespace Doc.Pulse.Core.Entities.ValueObjects;

public class EffectiveDateRange : ValueObject
{
    public DateTimeOffset Effective { get; init; }
    public DateTimeOffset Term { get; init; }

    private EffectiveDateRange(DateTimeOffset effective, DateTimeOffset term)
    {
        Effective = effective;
        Term = term;
    }

    public static EffectiveDateRange New()
    {
        var range = new EffectiveDateRange(TimeProvider.System.GetLocalNow(), DateTimeOffset.MaxValue);

        return range;
    }

    public static EffectiveDateRange New(TimeProvider clock)
    {
        var range = new EffectiveDateRange(clock.GetLocalNow(), DateTimeOffset.MaxValue);

        return range;
    }

    public static EffectiveDateRange? New(DateTimeOffset? effective, DateTimeOffset? term = null)
    {
        if (effective == null) return null;

        term ??= DateTimeOffset.MaxValue;

        var range = new EffectiveDateRange(effective.Value, term.Value);

        return range;
    }

    public static EffectiveDateRange New(DateTimeOffset effective, DateTimeOffset? term = null)
    {
        term ??= DateTimeOffset.MaxValue;

        var range = new EffectiveDateRange(effective, term.Value);

        return range;
    }

    public EffectiveDateRange AdjustTerm(DateTimeOffset? term)
    {
        term ??= DateTimeOffset.MaxValue;

        var range = new EffectiveDateRange(Effective, term.Value);

        return range;
    }

    public bool Overlap(EffectiveDateRange range)
    {
        return Effective < range.Term && Term > range.Effective;
    }

    public bool Overlap(IEnumerable<EffectiveDateRange> ranges)
    {
        return ranges.Any(range => Effective < range.Term && Term > range.Effective);
    }

    public bool OverlapsEndOnly(EffectiveDateRange range)
    {
        return range.Term == Term && Effective < range.Effective;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Effective;
        yield return Term;
    }
}
