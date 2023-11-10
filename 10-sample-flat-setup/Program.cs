using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .AddCommandLine(args)
    .Build();

Console.WriteLine($"markdownStyle: {configuration["markdownStyle"]}");
Console.WriteLine($"pdfOutputDir: {configuration["pdfOutputDir"]}");
Console.WriteLine($"pdfTemplateFile: {configuration["pdfTemplateFile"]}");
Console.WriteLine($"docOutputDir: {configuration["docOutputDir"]}");
Console.WriteLine($"docTemplateFile: {configuration["docTemplateFile"]}");
Console.WriteLine($"htmlOutputDir: {configuration["htmlOutputDir"]}");
Console.WriteLine($"htmlTemplateFile: {configuration["htmlTemplateFile"]}");
