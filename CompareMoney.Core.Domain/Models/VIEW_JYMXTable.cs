using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CompareMoney.Core.Domain.Models
{
    public class VIEW_JYMXTable
    {
        [Key]
        public string HISTRANSACTIONID { get; set; }//HIS交易号
        public string PAYTRANSACTIONID { get; set; } //Pay交易号
        public string THIRDTRANSACTIONID { get; set; } //第三方交易号
        public string PATIENTNAME { get; set; }
        public string PATIENTCARDNO { get; set; }
        public double? PATIENTTRADETYPE { get; set; }
        public int? ISREFUND { get; set; }
        public string TRADECOMMENT { get; set; }
        public string OPERATORID { get; set; }
        public string TRADEDATE { get; set; }
        public string TRADETIME { get; set; }
        public string BILLDATE { get; set; } //日期yyyy-MM-dd
        public double? TRADEID { get; set; }
        public double TRADEMONEY { get; set; }


    }
}
