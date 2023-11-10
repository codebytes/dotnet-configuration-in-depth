using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .AddCommandLine(args)
    .Build();


Console.WriteLine($"markdownStyle: {configuration["markdownStyle"]}");
Console.WriteLine($"pdf FileExtension: {configuration["pdf:FileExtension"]}");
Console.WriteLine($"pdf OutputDir: {configuration["pdf:OutputDir"]}");
Console.WriteLine($"pdf TemplateFile: {configuration["pdf:TemplateFile"]}");
Console.WriteLine($"doc FileExtension: {configuration["doc:FileExtension"]}");
Console.WriteLine($"doc OutputDir: {configuration["doc:OutputDir"]}");
Console.WriteLine($"doc TemplateFile: {configuration["doc:TemplateFile"]}");
Console.WriteLine($"html FileExtension: {configuration["html:FileExtension"]}");
Console.WriteLine($"html OutputDir: {configuration["html:OutputDir"]}");
Console.WriteLine($"html TemplateFile: {configuration["html:TemplateFile"]}");
