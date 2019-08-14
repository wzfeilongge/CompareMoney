using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CompareMoney.Core.Domain.Models
{
    public class OutMoneyTable
    {
        [Key]
        public string orderNo { get; set; }

        public string refundReason { get; set; }

        public string refundAmount { get; set; }

        public DateTime now { get; set; }
    }
}
