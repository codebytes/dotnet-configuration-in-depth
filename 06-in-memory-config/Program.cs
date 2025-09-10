using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>()
    {
        ["greeting"] = "Hello",
        ["environment"] = "dev"
    })
    .Build();

Console.WriteLine(configuration["SomeKey"]);
var name = "Chris";
// Outputs:
//   SomeValue
// See https://aka.ms/new-console-template for more information
Console.WriteLine($"{configuration["greeting"]}, {name}!");
Console.WriteLine($"Configuration: {configuration["environment"]}");
