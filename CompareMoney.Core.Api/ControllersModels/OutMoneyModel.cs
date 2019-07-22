using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareMoney.Core.Api.ControllersModels
{
    public class OutMoneyModel
    {


        /// <summary>
        /// 管理员密码
        /// </summary>
       public string AdminPassword { get; set; }

        /// <summary>
        /// 退款订单编号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string refundReason { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string refundAmount { get; set; }





    }
}
