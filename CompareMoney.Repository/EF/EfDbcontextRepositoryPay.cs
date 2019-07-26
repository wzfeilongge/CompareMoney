using CompareMoney.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompareMoney.Repository.Base
{
   public class EfDbcontextRepositoryPay : DbContext
    {
        public static EfDbcontextRepositoryPay Context
        {
            get
            {

                return new EfDbcontextRepositoryPay();

            }
        }

        public EfDbcontextRepositoryPay()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var sqlType = config["PaySql:sqlType"];
            var sqlstr = config["PaySql:str"];

            if (sqlType == "1")
            {

                optionsBuilder.UseSqlServer(sqlstr, b => b.UseRowNumberForPaging());
                Console.WriteLine("Pay是sqlserver");
                return;
            }
            else if (sqlType == "2")
            {
                optionsBuilder.UseOracle(sqlstr);
                Console.WriteLine("His是Oracle");
                return;

            }


        }

        public DbSet<FXStmtLine> FXStmtLine { get; set; }  //pay 的数据
        public DbSet<PayTable> PayTable { get; set; }  //pay 的数据
        public DbSet<User> User{ get; set; }  //pay 的数据






    }
}
