using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompareMoney.Core.Domain.Models
{
    public class PayTable
    {
        [Key]
        public string bankTrxnNo { get; set; }

        public string orderNo { get; set; }

        public string trxNo { get; set; }

        public string orderDate { get; set; }

        public string payWayCode { get; set; }

        public string payWayName { get; set; }

        public string orderTime { get; set; }

        public string orderAmount { get; set; }

        public string productName { get; set; }

        public string isRefund { get; set; }

        public string refundAmount { get; set; }
    }
}