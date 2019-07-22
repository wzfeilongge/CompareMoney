using CompareMoney.Core.Domain.Models;

using IServices.BaseServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.IServices
{
   public interface  IVIEW_JYMXTableServices : IBaseEntityInforHis<VIEW_JYMXTable>
    {


        Task<List<VIEW_JYMXTable>> GetOneyDay(string Billdate);



    }
}
