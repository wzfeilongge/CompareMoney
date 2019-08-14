using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompareMoney.Business.Services;
using CompareMoney.Business.Services.Domain;
using CompareMoney.Core.Api.Filter;
using CompareMoney.Core.Api.JwtHelper;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Profiling.Storage;
using Swashbuckle.AspNetCore.Swagger;

namespace CompareMoney.Core.Api
{
    public class Startup
    {
        public static ILoggerRepository Repository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Repository = LogManager.CreateRepository("CompareMoney.Core");
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionFilter)); //注入异常
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region Token注入
                
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("管理员").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("管理员", "System"));
                options.AddPolicy("Guest",policy=>policy.RequireRole("Guest").Build());
            });

            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,//还是从 appsettings.json 拿到的
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],//发行人
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            services.AddAuthentication("Bearer")
            .AddJwtBearer(o =>
            {
               o.TokenValidationParameters = tokenValidationParameters;
             });
            #endregion

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

                #region Token绑定到ConfigureServices

                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Blog.Core", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("CompareMoney.Api", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });

                #endregion




            });

            #endregion

            #region 注入Services 和全局异常日志

            services.AddSingleton<ILoggerHelper, LogHelper>(); //注入全局日志
            services.AddSingleton<IFXStmtLineServices, FXStmtLineServices>(); //
            services.AddSingleton<IPayTableServices, PayTableServices>(); //
            services.AddSingleton<IUserServices, UserServices>(); //
            services.AddSingleton<IVIEW_JYMXTableServices, VIEW_JYMXTableServices>(); //
            services.AddSingleton<ICompareMoneyInterface, CompareMoenyHandle>(); //
         //   services.AddSingleton<IDownLoadInterface, DownLoadlHandle>(); //


            #endregion

            #region 注入仓储
            services.AddSingleton<IFXStmtLineRepository, FXStmtLineRepository>(); //
            services.AddSingleton<IPayTableRepository, PayTableRepository>(); //我方Pay仓储
            services.AddSingleton<IUserRepository, UserRepository>(); //用户仓储
            services.AddSingleton<IVIEW_JYMXTableRepository, VIEW_JYMXTableRepository>(); //His数据仓储
            services.AddSingleton<IOutMoneyRepository, OutMoneyRepository>(); //退费仓储
            #endregion

            #region  注入DB

            services.AddScoped<EfDbcontextRepository>();

            services.AddScoped<EfDbcontextRepositoryPay>();

            #endregion
        }

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

            #region 解决跨域问题
            app.Use(async (context, next) =>
            {
                context.Response.Headers.SetCommaSeparatedValues("Access-Control-Allow-Origin", "*");
                await next();
            });

            #endregion

            #region token机制
           //
           // app.UseJwtTokenAuth();
            app.UseAuthentication();
            #endregion

            #region 短板中间件
            app.UseMvc();

            #endregion
        }
    }
}
