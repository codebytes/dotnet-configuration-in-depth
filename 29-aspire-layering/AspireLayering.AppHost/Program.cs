var builder = DistributedApplication.CreateBuilder(args);

// Parameters (can be overridden via env vars or command line). Use string default for interval to match AddParameter overload.
var apiBaseParam = builder.AddParameter("ApiBaseMessage", "Hello from Parameter (default)");
var workerIntervalParam = builder.AddParameter("WorkerIntervalSeconds", "3");

// Simulated secret placeholder (override via user secrets or environment var / user secrets)
var apiKeyParam = builder.AddParameter("ExternalApiKey", "__REPLACE_ME__");

// API service: demonstrates env parameter override of a value and secret mapping
builder.AddProject<Projects.AspireLayering_Api>("api")
    .WithEnvironment("Api__InjectedMessage", apiBaseParam)
    .WithEnvironment("Api__ExternalApiKey", apiKeyParam);

// Worker service: demonstrates interval + secret + shared message layering
builder.AddProject<Projects.AspireLayering_Worker>("worker")
    .WithEnvironment("Worker__IntervalSeconds", workerIntervalParam)
    .WithEnvironment("Worker__ExternalApiKey", apiKeyParam);

builder.Build().Run();
