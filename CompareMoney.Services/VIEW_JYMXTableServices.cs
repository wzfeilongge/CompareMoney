using CompareMoney.Core.Domain.Models;
using CompareMoney.IServices;
using CompareMoney.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.Services
{
    public class VIEW_JYMXTableServices : BaseServicesInforHis<VIEW_JYMXTable>, IVIEW_JYMXTableServices
    {

       private readonly  IVIEW_JYMXTableServices _VIEW_JYMXTableServices;


        public VIEW_JYMXTableServices(IVIEW_JYMXTableServices vIEW_JYMXTableServices)
        {
            _VIEW_JYMXTableServices = vIEW_JYMXTableServices;
        }



        public async  Task<List<VIEW_JYMXTable>> GetOneyDay(string Billdate)
        {


            return await _VIEW_JYMXTableServices.Query(obj => obj.BILLDATE == Billdate); ;


        }
    }
}
