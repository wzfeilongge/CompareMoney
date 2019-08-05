using System;
using System.Collections.Generic;
using System.Text;

namespace CompareMoney.Core.Domain.Models
{
    public class OutMoneyTable
    {
        public string orderNo { get; set; }

        public string refundReason { get; set; }

        public string refundAmount { get; set; }

        public DateTime now { get; set; }
    }
}
