﻿using Autofac;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SimApi.Base;
using SimApi.Data.Repository;
using SimApi.Data.Uow;
using SimApi.Operation;
using SimApi.Service.Middleware;
using SimApi.Service.RestExtension;

namespace SimApi.Service;

public class Startup 
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public static JwtConfig JwtConfig { get; private set; }


    public void ConfigureServices(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(Startup).Assembly)
            .Where(x => typeof(ControllerBase).IsAssignableFrom(x))
            .AsImplementedInterfaces();
         JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        builder.RegisterInstance(JwtConfig).As<JwtConfig>();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DefaultModelsExpandDepth(-1);
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sim Company");
            c.DocumentTitle = "SimApi Company";
        });

        //DI
        app.AddExceptionHandler();
        app.AddDIExtension();

        app.UseMiddleware<HeartBeatMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        Action<RequestProfilerModel> requestResponseHandler = requestProfilerModel =>
        {
            Log.Information("-------------Request-Begin------------");
            Log.Information(requestProfilerModel.Request);
            Log.Information(Environment.NewLine);
            Log.Information(requestProfilerModel.Response);
            Log.Information("-------------Request-End------------");
        };
        app.UseMiddleware<RequestLoggingMiddleware>(requestResponseHandler);

        app.UseHttpsRedirection();

        // add auth 
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
