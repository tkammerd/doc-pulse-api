// See https://aka.ms/new-console-template for more information
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Core.Entities._Kernel;
using Doc.Pulse.DatabaseLoader;
using Doc.Pulse.DatabaseLoader.SeedModels;

var serviceProvider = Helpers.Setup();

var accountOrganizationsPath = new FileInfo(@"data\AccountOrganizations.csv");
var agenciesPath = new FileInfo(@"data\Agencies.csv");
var programsPath = new FileInfo(@"data\Programs.csv");
var codeCategoriesPath = new FileInfo(@"data\CodeCategories.csv");
var objectCodesPath = new FileInfo(@"data\ObjectCodes.csv");
var vendorsPath = new FileInfo(@"data\Vendors.csv");
var appropriationsPath = new FileInfo(@"data\Appropriations.csv");
var rfpsPath = new FileInfo(@"data\Rfps.csv");
var receiptsPath = new FileInfo(@"data\Receipts.csv");
var userStubsPath = new FileInfo(@"data\UserStubs.csv");

Console.WriteLine("Running Data Purge and Seed!");

var accountOrganizations = Helpers.ParseRecords<AccountOrganizationDto>(accountOrganizationsPath.FullName)
    .Select(o => o.ToEntity<AccountOrganization>()).ToList();
var agencies = Helpers.ParseRecords<AgencyDto>(agenciesPath.FullName)
    .Select(o => o.ToEntity<Agency>()).ToList();
var programs = Helpers.ParseRecords<ProgramDto>(programsPath.FullName)
    .Select(o => o.ToEntity<Doc.Pulse.Core.Entities.Program>()).ToList();
var codeCategories = Helpers.ParseRecords<CodeCategoryDto>(codeCategoriesPath.FullName)
    .Select(o => o.ToEntity<CodeCategory>()).ToList();
var objectCodes = Helpers.ParseRecords<ObjectCodeDto>(objectCodesPath.FullName)
    .Select(o => o.ToEntity<ObjectCode>()).ToList();
var vendors = Helpers.ParseRecords<VendorDto>(vendorsPath.FullName)
    .Select(o => o.ToEntity<Vendor>()).ToList();
var appropriations = Helpers.ParseRecords<AppropriationDto>(appropriationsPath.FullName)
    .Select(o => o.ToEntity<Appropriation>()).ToList();
var rfps = Helpers.ParseRecords<RfpDto>(rfpsPath.FullName)
    .Select(o => o.ToEntity<Rfp>()).ToList();
var receipts = Helpers.ParseRecords<ReceiptDto>(receiptsPath.FullName)
    .Select(o => o.ToEntity<Receipt>()).ToList();
var userStubs = Helpers.ParseRecords<UserStubDto>(userStubsPath.FullName)
    .Select(o => o.ToEntity<UserStub>()).ToList();

Console.WriteLine("====================================");

serviceProvider
    .WipeoutDatabase();

Console.WriteLine($"Data Purge Committed!");
Console.WriteLine("------------------------------------");

Console.WriteLine("Seeding Data...");
// Seed from least dependent to most dependent
serviceProvider
    .SeedIdentifiedData(userStubs)
    .SeedIdentifiedData(accountOrganizations)
    .SeedIdentifiedData(agencies)
    .SeedIdentifiedData(programs)
    .SeedIdentifiedData(vendors)
    .SeedIdentifiedData(codeCategories)
    .SeedIdentifiedData(objectCodes)
    .SeedIdentifiedData(rfps)
    .SeedUnidentifiedData(appropriations)
    .SeedUnidentifiedData(receipts)
    ;

Console.WriteLine($"Data Seed Committed!");
Console.WriteLine("====================================");
Console.WriteLine("Run Complete....");