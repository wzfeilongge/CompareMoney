using CompareMoney.Core.Domain.Models;

using CompareMoney.IRepository;
using CompareMoney.Repository.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompareMoney.Repository
{
   public class PayTableRepository : BaseRepositoryPay<PayTable>, IPayTableRepository
    {

        public PayTableRepository(ILogger<BaseRepositoryPay<PayTable>> logger) : base(logger)
        {

        }

    }
}
