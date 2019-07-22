using CompareMoney.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompareMoney.Repository.EF
{
   public class EfDbcontextRepository:DbContext
    {

        public EfDbcontextRepository()
        {

        }

        public static EfDbcontextRepository Context { get {

                return new EfDbcontextRepository();

            } }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var sqlType = config["HisSql:sqlType"];
            var sqlstr = config["HisSql:str"];

            if (sqlType == "1")
            {

                optionsBuilder.UseSqlServer(sqlstr, b => b.UseRowNumberForPaging());
                Console.WriteLine("His是sqlserver");
                return;
            }
            else if (sqlType == "2")
            {
                optionsBuilder.UseOracle(sqlstr);
                Console.WriteLine("His是Oracle");
                return;

            }


        }
        public DbSet<VIEW_JYMXTable> VIEW_JYMXTable { get; set; }  //His 的数据








    }
}
