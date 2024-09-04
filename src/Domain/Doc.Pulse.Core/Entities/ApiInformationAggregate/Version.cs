namespace Doc.Pulse.Core.Entities.ApiInformationAggregate;

public abstract class Version //BaseEntity<string>
{
    public string Id { get; set; } = default!;
    public string Description { get; set; } = string.Empty;

    protected Version(string revenueCodeTypeId, string description)
    {
        Id = revenueCodeTypeId;
        Description = description;
    }

    //public static readonly Version IsisRevenueCode = IsisRevenueCodeType.New("ISIS", "Isis Revenue Code");
    //public static readonly Version LagovRevenueCode = LagovRevenueCodeType.New("LAGOV", "Lagov Revenue Code");

    //public RevenueCodeType New(string revenueCodeTypeId, string description)
    //{
    //    var entity = new RevenueCodeType(revenueCodeTypeId, description);

    //    return entity;
    //}

    public abstract string CalculateRevenueCodeKey(object revenueCodeDto);



    //private class IsisRevenueCodeType : Version
    //{
    //    protected IsisRevenueCodeType(string revenueCodeTypeId, string description) : base(revenueCodeTypeId, description) { }

    //    public static Version New(string revenueCodeTypeId, string description)
    //    {
    //        var entity = new IsisRevenueCodeType(revenueCodeTypeId, description);

    //        return entity;
    //    }

    //    public override string CalculateRevenueCodeKey(object revenueCodeDto) //"I-419-0000-0001-01-5555"
    //    {
    //        if (revenueCodeDto is IsisCodeKey code)
    //        {
    //            return $"{code.IsisAgencyNumber}-{code.Org}-{code.RevSource}-{code.RevSubSource}-{code.ReportingCategory}";
    //        }

    //        throw new Exception($"Error: Unable to Calculate Revenue Code. Attempting to Generate an ISIS Code, but object of type {revenueCodeDto.GetType()} was passed as a parameter.");
    //    }
    //}

    //private class LagovRevenueCodeType : Version
    //{
    //    protected LagovRevenueCodeType(string revenueCodeTypeId, string description) : base(revenueCodeTypeId, description) { }

    //    public static Version New(string revenueCodeTypeId, string description)
    //    {
    //        var entity = new LagovRevenueCodeType(revenueCodeTypeId, description);

    //        return entity;
    //    }

    //    public override string CalculateRevenueCodeKey(object revenueCodeDto) //"I-419-000000-000000000-000000000-000000000000"
    //    {
    //        if (revenueCodeDto is LaGovCodeKey code)
    //        {
    //            return $"{code.BusinessArea}-{code.GeneralLedgerNumber}-{code.Fund}-{code.Center}-{code.Order}";
    //        }

    //        throw new Exception($"Error: Unable to Calculate Revenue Code. Attempting to Generate an LAGOV Code, but object of type {revenueCodeDto.GetType()} was passed as a parameter.");
    //    }
    //}
}