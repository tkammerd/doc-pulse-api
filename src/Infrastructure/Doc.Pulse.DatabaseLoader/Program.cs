// See https://aka.ms/new-console-template for more information
using Doc.Pulse.Core.Entities;
using Doc.Pulse.DatabaseLoader;
using Doc.Pulse.DatabaseLoader.SeedModels;

var serviceProvider = Helpers.Setup();

var codeCategoriesPath = new FileInfo(@"data\CodeCategories.csv");
var objectCodesPath = new FileInfo(@"data\ObjectCodes.csv");

Console.WriteLine("Running Data Purge and Seed!");

var codeCategories = Helpers.ParseRecords<CodeCategoryDto>(codeCategoriesPath.FullName).Select(o => o.ToEntity<CodeCategory>()).ToList();

var objectCodes = Helpers.ParseRecords<ObjectCodeDto>(objectCodesPath.FullName).Select(o => o.ToEntity<ObjectCode>()).ToList();

Console.WriteLine("====================================");

serviceProvider
    .WipeoutDatabase();

Console.WriteLine($"Data Purge Committed!");
Console.WriteLine("------------------------------------");

Console.WriteLine("Seeding Data...");
serviceProvider
    .SeedData(codeCategories)
    .SeedData(objectCodes)
    ;

Console.WriteLine($"Data Seed Committed!");
Console.WriteLine("====================================");
Console.WriteLine("Run Complete....");