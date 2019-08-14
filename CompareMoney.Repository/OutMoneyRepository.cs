using CompareMoney.Core.Domain.Models;
using CompareMoney.IRepository;
using CompareMoney.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompareMoney.Repository
{
   public class OutMoneyRepository : BaseRepositoryPay<OutMoneyTable>, IOutMoneyRepository
    {
    }
}
