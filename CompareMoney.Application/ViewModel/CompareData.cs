using System;
using System.Collections.Generic;
using System.Text;

namespace CompareMoney.Applicaion.ViewModel
{
     public class CompareData
    {
        public string tradeMoney { get; set; } //交易金额

        public string BillDate { get; set; } //时间
        public string hisTransactionId { get; set; } //His号

        public string payTransTransactionId { get; set; } //支付平台流水号

        public string thirdTransactionId { get; set; }  //第三方流水号

        public string patientName { get; set; } //病人姓名
        public string pateontID { get; set; }  //病人唯一Id

        public string pateontCardNo { get; set; } //病人卡号


        public int pateintTradeType { get; set; }  //交易类型  1 支付宝 2微信  3被动扫码支付宝 4被动扫码微信  5支付宝扫脸  6微信扫脸 7银行卡刷卡 8银联扫码 9 聚合支付 10 现金

        public string transactionTime { get; set; }
        public int? IsRefund { get; set; } //交易类型

        //  public string Remarks { get; set; } //退费备注
        public string tradeComment { get; set; } //购买备注


        public string operatorId { get; set; } //操作员号


        public int isTrue { get; set; } //交易是否正常

        public string tradeTime { get; set; }

        public string orderNo { get; set; }//订单编号




    }
}
