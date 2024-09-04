using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Doc.Pulse.Tests.UnitTests;
public class PlayGround
{
    private class TestClass(string x)
    {
        public string X { get; set; } = x;
        public TestSubClass Y { get; set; } = new();
    }
    private class TestSubClass
    {
        public string X { get; set; } = "XXXXXX";
        public string Y { get; set; } = "YYYYYY";
    }

    [Fact]
    public void TestWhatever()
    {
        try
        {
            //var connectionString = "Data Source=s-dmdb-pulse01.swe.la.gov;Initial Catalog=PULSE;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            //var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            //optionsBuilder.UseSqlServer(connectionString);
            //var context = new AppDbContext(optionsBuilder.Options);
            //var foo = context.GlElements.Where(x => x.ElementSpecificId == "DPSSG2000113");             //  .Include(x => x.LaGovCodes).ToList();
            //                                                                                            // var bar = context.GlElements.Find(318);

            //var code = context.LaGovCodes.Where(x=>x.Id == 1).Include(x=>x.Fund).First();


        }
        catch (Exception)
        {

            throw;
        }
    }

    [Fact]
    public void TestExists()
    {
        bool ggg = File.Exists(@"C:\windows\system32\lpr.exe");
        System.Console.WriteLine(ggg);

        if (File.Exists(@"C:\windows\system32\lpr.exe"))
        {
            var ggg2 = false;
            System.Console.WriteLine(ggg2);
        }

    }
}
