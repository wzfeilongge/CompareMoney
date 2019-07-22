using CompareMoney.Core.Domain.Models;
using IServices.BaseServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.IServices
{
    public interface IPayTableServices: IBaseEntityInforPay<PayTable>
    {

        Task<List<PayTable>> GetOneyDay(string Billdate);//一天的 



    }
}
