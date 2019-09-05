using CompareMoney.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompareMoney.Repository.EF
{
    public class EfDbcontextRepository : DbContext
    {      
        public static EfDbcontextRepository Context
        {
            get
            {
                return new EfDbcontextRepository();
            }
        }
     
      
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            this.Database.EnsureCreated();
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var sqlType = config["HisSql:sqlType"];
            var sqlstr = config["HisSql:str"];
            if (sqlType == "1")
            {
               // optionsBuilder.UseLoggerFactory(_myLogger);
                optionsBuilder.UseSqlServer(sqlstr, b => b.UseRowNumberForPaging());
                Console.WriteLine("His是sqlserver");
                return;
            }
            else if (sqlType == "2")
            {
              //optionsBuilder.UseLoggerFactory(logger);
                optionsBuilder.UseOracle(sqlstr);
              Console.WriteLine("His是Oracle");
                return;
            }
            else if (sqlType=="3") {
               // optionsBuilder.UseLoggerFactory(logger);
                optionsBuilder.UseMySQL(sqlstr);
                Console.WriteLine("His是Mysql");
                return;
            }


        }

        public DbSet<VIEW_JYMX> VIEW_JYMX { get; set; }  //His 的数据
    }
}
