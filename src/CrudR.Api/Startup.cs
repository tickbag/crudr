﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;
using CrudR.Api.Extensions;
using CrudR.Api.Filters;
using CrudR.Api.Middleware;
using CrudR.Api.Models;
using CrudR.Api.Options;
using CrudR.Api.Swagger;
using CrudR.Core;
using CrudR.DAL;
using CrudR.DAL.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CrudR.Api
{
    /// <summary>
    /// Startup class encompassing application initialisation code
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Startup
    {
        private const string ApiVersion = "v1";
        private const string ApiTitle = "CrudR API";
        private const string ApiDescription = "Generic CRUD REST API for storing any static data structure";

        /// <summary>
        /// The Startup constructor
        /// </summary>
        /// <param name="configuration">The configuration instance</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// The configuration instance for the application
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure service routine
        /// </summary>
        /// <param name="services">The service collection instance</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(RevisionActionFilter));
                options.Filters.Add(typeof(ValidateModelStateFilter));
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            var appOptions = Configuration.GetSection(nameof(ApplicationOptions))
                .GetValidated<ApplicationOptions>();
            services.AddSingleton<IApplicationOptions>(appOptions);

            var dbOptions = Configuration.GetSection(nameof(DatabaseOptions))
                .GetValidated<DatabaseOptions>();
            services.AddSingleton<IDatabaseOptions>(dbOptions);

            // Configure Swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiVersion, new OpenApiInfo
                {
                    Version = ApiVersion,
                    Title = ApiTitle,
                    Description = ApiDescription,
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.MapType<JsonElement>(() => new OpenApiSchema { Type = "object" });
                c.OperationFilter<RevisionHeaderParameterOperationFilter<RevisionContext>>();
            });

            services.AddApiServices();
            services.AddCoreServices();
            services.AddMongoDataAccessLayer();
        }

        /// <summary>
        /// Configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">The Application Builder instance</param>
        /// <param name="env">The Web Host Environment instance</param>
        /// <param name="appOptions">Application Options instance</param>
        public static void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IApplicationOptions appOptions)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseSwagger();

            if (appOptions.EnableSwaggerUI)
            {
                app.UseSwaggerUI(cfg =>
                {
                    cfg.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", ApiTitle);
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}