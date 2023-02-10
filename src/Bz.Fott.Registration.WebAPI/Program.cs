using Bz.Fott.Registration.Application;
using Bz.Fott.Registration.Infrastructure;
using Bz.Fott.Registration.WebAPI;

const string serviceName = "Fott-Registration";
const string serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.
services
    .AddObservability(config, serviceName, serviceVersion)
    .AddApplication()
    .AddInfrastructure(config)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCustomControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();
}

app.UseHttpsRedirection()
   .UseAuthorization();

app.MapControllers();

app.MigrateDb();

app.Run();
