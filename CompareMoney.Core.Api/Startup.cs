﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CompareMoney.Business.Services.Domain;
using CompareMoney.Core.Api.Filter;
using CompareMoney.Core.Api.Log;
using CompareMoney.IRepository;
using CompareMoney.IRepository.BaseRepository;
using CompareMoney.IServices;
using CompareMoney.Repository;
using CompareMoney.Repository.Base;
using CompareMoney.Repository.EF;
using CompareMoney.Services;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Profiling.Storage;
using Swashbuckle.AspNetCore.Swagger;

namespace CompareMoney.Core.Api
{
    public class Startup
    {


        public static ILoggerRepository repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;


            repository = LogManager.CreateRepository("CompareMoney.Core");


            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionFilter)); //注入异常


            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMiniProfiler(options =>
                {
                    options.RouteBasePath = "/profiler";
                    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(10);

                }
            );


            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "自动对账系统 API",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "自动对账系统",
                        Email = "wzfeilongge@tencent.me",
                        Url = ""
                    }
                });

                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "CompareMoney.Core.Api.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改


            });

            #endregion

            services.AddSingleton<ILoggerHelper, LogHelper>(); //注入全局日志

            //services.AddMiniProfiler(options =>
            //    {
            //        options.RouteBasePath = "/profiler";
            //        (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(10);

            //    }
            //);




            #region 注入Services


            services.AddSingleton<IFXStmtLineServices, FXStmtLineServices>(); //
            services.AddSingleton<IPayTableServices, PayTableServices>(); //
            services.AddSingleton<IUserServices, UserServices>(); //
            services.AddSingleton<IFXStmtLineServices, FXStmtLineServices>(); //
            services.AddSingleton<IVIEW_JYMXTableServices, VIEW_JYMXTableServices>(); //
            services.AddSingleton<CompareMoneyInterface, CompareMoenyHandle>(); //

            #endregion


            #region 注入仓储

            services.AddSingleton<IFXStmtLineRepository, FXStmtLineRepository>(); //注入全局日志
            services.AddSingleton<IPayTableRepository, PayTableRepository>(); //注入全局日志
            services.AddSingleton<IUserRepository, UserRepository>(); //注入全局日志
            services.AddSingleton<IVIEW_JYMXTableRepository, VIEW_JYMXTableRepository>(); //注入全局日志




            #endregion


            #region  注入DB
            // EfDbcontextRepository
            services.AddScoped<EfDbcontextRepository>();

            services.AddScoped<EfDbcontextRepositoryPay>();

            #endregion



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "自动对账Api V1");


                c.RoutePrefix = "";

            });
            #endregion

            app.UseMvc();



        }
    }
}
