using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareMoney.Core.Api.ControllersModels
{
    public class GetBillPageModel
    {


        /// <summary>
        /// 日期数组
        /// </summary>
        public string[] BillDate { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNo { get; set; } = 2;

        /// <summary>
        /// 页码大小
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
