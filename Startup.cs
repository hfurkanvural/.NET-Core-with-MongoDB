using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using docker_api.Models;
using Prometheus;

namespace docker_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // Log.Logger = new LoggerConfiguration()
            //     .Enrich.FromLogContext()
            //     .Enrich.WithExceptionDetails()
            //     .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            //     {
            //         AutoRegisterTemplate = true,
            //     })
            // .CreateLogger();
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ServerConfig();
            Configuration.Bind(config);
            
            var knowledgeContext = new KnowledgeContext(config.MongoDB);
            var repo = new KnowledgeRepository(knowledgeContext);
            services.AddSingleton<IKnowledgeRepository>(repo);
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:5000","https://localhost:5000","http://localhost:5001","https://localhost:5001")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            // services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use((context, next) =>
            {
                // Http Context
                var counter = Metrics.CreateCounter
                ("PathCounter", "Count request", 
                new CounterConfiguration{
                    LabelNames = new [] {"method", "endpoint"}
                }); 
                // method: GET, POST etc.
                // endpoint: Requested path
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins); 

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseMetricServer();
        }
    }
}
