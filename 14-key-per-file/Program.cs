using Microsoft.Extensions.Configuration;

   var path = Path.Combine(
        Directory.GetCurrentDirectory(), "./keyPerFile");

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddKeyPerFile(directoryPath: path, optional: true)
    .AddUserSecrets<Program>(optional:true)
    .Build();

var name = "World";
Console.WriteLine($"{configuration["greeting"]}, {name}.");
Console.WriteLine($"Configuration: {configuration["environment"]}");

var debugView = configuration.GetDebugView();
Console.WriteLine(debugView.ToString());