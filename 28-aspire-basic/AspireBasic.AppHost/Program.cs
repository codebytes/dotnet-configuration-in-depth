var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireBasic_ApiService>("apiservice");

builder.AddProject<Projects.AspireBasic_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
