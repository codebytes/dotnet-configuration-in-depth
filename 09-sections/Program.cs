using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .AddCommandLine(args)
    .Build();


Console.WriteLine($"markdownStyle: {configuration["markdownStyle"]}");

var options = configuration.GetSection("pdf");
WriteOptions(options);
options = configuration.GetSection("doc");
WriteOptions(options);
options = configuration.GetSection("html");
WriteOptions(options);


void WriteOptions(IConfiguration config)
{
    var fileExtension = config["fileExtension"];
    Console.WriteLine($"{fileExtension} fileExtension: {fileExtension}");
    Console.WriteLine($"{fileExtension} OutputDir: {config["OutputDir"]}");
    Console.WriteLine($"{fileExtension} TemplateFile: {config["TemplateFile"]}");
}