using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Doc.Pulse.DatabaseLoader.SeedModels
{
    internal class TestDto
    {
        [Index(0)] public string AAA { get; set; } = "";
        [Index(1)] public string BBB { get; set; } = "";
        [Index(2)] public string CCC { get; set; } = "";
    }
}
