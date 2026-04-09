using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .AddCommandLine(args)
    .Build();


Console.WriteLine($"markdownStyle: {configuration["markdownStyle"]}");

var pdfOptions = new OutputStyle();
configuration.GetSection("pdf")
    .Bind(pdfOptions);
WriteOptions(pdfOptions);

var docOptions = configuration.GetSection("doc")
    .Get<OutputStyle>();
WriteOptions(docOptions);

var htmlOptions = configuration.GetSection("html")
    .Get<OutputStyle>();
WriteOptions(htmlOptions);


void WriteOptions(OutputStyle? options)
{
    Console.WriteLine($"{options?.FileExtension} fileExtension: {options?.FileExtension}");
    Console.WriteLine($"{options?.FileExtension} OutputDir: {options?.OutputDir}");
    Console.WriteLine($"{options?.FileExtension} TemplateFile: {options?.TemplateFile}");
}


public class OutputStyle
{
    public string FileExtension { get; set; } ="";
    public string OutputDir { get; set; } ="";
    public string TemplateFile { get; set; } ="";
}