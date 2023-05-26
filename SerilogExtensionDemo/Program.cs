using Serilog;
using SerilogExtensionDemo.Infrastructure;
using Serilog.SharedLogContext;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostBuilderContext, services, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Properties:j}{NewLine}{Exception}");
    loggerConfiguration.WriteTo.Seq("", apiKey: "");
    
    loggerConfiguration.Enrich.WithProperty("Environment", hostBuilderContext.HostingEnvironment.EnvironmentName);

    // configure shared context enricher
    loggerConfiguration.Enrich.FromSharedLogContext(services);
});


builder.Services.AddControllers();

// register shared context services
builder.Services.AddSharedLogContext();
builder.Services.AddSingleton<ISharedLogContextAdapter, SharedLogContextAdapter>();
builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

// add shared context middleware before any other midllewares
app.UseSharedLogContext();

app.UseSerilogRequestLogging();
app.UseRouting()
   .UseEndpoints(x => x.MapControllers());

app.Run();