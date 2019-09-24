using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;


namespace CityInfo.API
{
    public class Startup
    {
        public static IConfiguration Configuration;


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appSettings.json", optional:false, reloadOnChange:true);

            //Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()
                    ));
            //Enable serialization
            //    .AddJsonOptions(o => { 
            //    if(o.SerializerSettings.ContractResolver != null)
            //        {
            //            var castedResolver = o.SerializerSettings.ContractResolver
            //                as DefaultContractResolver;
            //            castedResolver.NamingStrategy = null;
            //        }

            //});

#if DEBUG
            services.AddTransient<IMailService, LocalMailServices>();
#else
            services.AddTransient<IMailService, CloudMailServices>();
#endif
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"]; //@"Server =(localdb)\mssqllocaldb;Database=CityInfoDB;Trusted_Connection=True;";
             services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            CityInfoContext cityInfoContext)
        {
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            cityInfoContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Entities.City, Models.CityWithoutPointOfInterestDto>();
            //});

            //AutoMapper.Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<Entities.City, Models.CityWithoutPointOfInterestDto>();
            //});

            app.UseMvc();
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
