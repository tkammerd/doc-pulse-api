using CsvHelper.Configuration.Attributes;

namespace Doc.Pulse.DatabaseLoader.SeedModels
{
    internal class TestDto
    {
        [Index(0)] public string AAA { get; set; } = "";
        [Index(1)] public string BBB { get; set; } = "";
        [Index(2)] public string CCC { get; set; } = "";
    }
}
