
using CompareMoney.Core.Domain.Models;
using CompareMoney.IRepository;
using CompareMoney.IServices;
using CompareMoney.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.Services
{
   public class PayTableServices: BaseServicesInforPay<PayTable>,IPayTableServices
    {
        private readonly IPayTableRepository _IPayTableServices;

        public PayTableServices(IPayTableRepository payTableServices)
        {
            _IPayTableServices = payTableServices;
        }

        public  async Task<List<PayTable>> GetOneyDay(string Billdate)
        {
            return await _IPayTableServices.Query(obj => obj.orderDate == Billdate);        
        }
    }
}
