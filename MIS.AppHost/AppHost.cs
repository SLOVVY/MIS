var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var postgresdb = postgres.AddDatabase("postgresdb");

var migrations = builder.AddProject<Projects.MIS_MigrationService>("migrations")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

var api = builder.AddProject<Projects.MIS_API>("api")
    .WithReference(postgresdb)
    .WaitForCompletion(migrations)
    .WithUrl("https://localhost:7227/swagger", "Swagger UI");

builder.Build().Run();