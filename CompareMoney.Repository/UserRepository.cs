using CompareMoney.Core.Domain.Models;

using CompareMoney.IRepository;
using CompareMoney.Repository.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompareMoney.Repository
{
   public class UserRepository : BaseRepositoryPay<User>, IUserRepository
    {
        //private static readonly ILogger<BaseRepositoryPay<User>> _logger;
        public UserRepository(ILogger<BaseRepositoryPay<User>> logger) :base(logger)
        {
           
        }

    }
}
