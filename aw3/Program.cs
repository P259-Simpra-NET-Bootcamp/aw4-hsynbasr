using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using SimApi.Service.Autofac;

namespace SimApi.Service;

public class Program
{
    public static void Main(string[] args)
    {

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
        Log.Information("Application is starting...");

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new AutofacModule());
        }
        )
        .UseSerilog()
        .ConfigureWebHostDefaults(builder =>
        {
            builder.UseStartup<Startup>();
        }
        );
}
