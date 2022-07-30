using Microsoft.Extensions.Configuration;

   var path = Path.Combine(
        Directory.GetCurrentDirectory(), "./keyPerFile");

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddKeyPerFile(directoryPath: path, optional: true)
    .Build();

var name = "World";
// See https://aka.ms/new-console-template for more information


//$Env:ENVIRONMENT="qa"; dotnet run  
//$Env:ENVIRONMENT="production"; dotnet run
//$Env:ENVIRONMENT=$NULL

Console.WriteLine($"{configuration["greeting"]}, {name}.");
Console.WriteLine($"Configuration: {configuration["environment"]}");

var debugView = configuration.GetDebugView();
Console.WriteLine(debugView.ToString());