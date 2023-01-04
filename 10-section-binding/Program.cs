using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .AddCommandLine(args)
    .Build();


Console.WriteLine($"markdownStyle: {configuration["markdownStyle"]}");

var pdfOptions = new FileOptions();
configuration.GetSection("pdf")
    .Bind(pdfOptions);
WriteOptions(pdfOptions);

var docOptions = configuration.GetSection("doc")
    .Get<FileOptions>();
WriteOptions(docOptions);

var htmlOptions = configuration.GetSection("html")
    .Get<FileOptions>();
WriteOptions(htmlOptions);


void WriteOptions(FileOptions? options)
{
    Console.WriteLine($"{options?.FileExtension} fileExtension: {options?.FileExtension}");
    Console.WriteLine($"{options?.FileExtension} OutputDir: {options?.OutputDir}");
    Console.WriteLine($"{options?.FileExtension} TemplateFile: {options?.TemplateFile}");
}


public class FileOptions
{
    public string FileExtension { get; set; } ="";
    public string OutputDir { get; set; } ="";
    public string TemplateFile { get; set; } ="";
}