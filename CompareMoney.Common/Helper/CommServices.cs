using CompareMoney.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompareMoney.Common.Helper
{
  public  class CommServices
    {
        public static bool Refund(string refundAmount, string orderNo, string refundResason, string PayKey = "b2f752964f904e6a9ad9397c3ded2e28", string Secret = "c37ae8ec310b4a68881ec49473d571a4")
        {
            FenXiangService service = new FenXiangService();
            Dictionary<string, string> para = new Dictionary<string, string>
            {
                { FenXiangService.REFUND_URL, "https://pay.xuhuihealth.cn/fx-pay-web-gateway/refund/doRefund" },
                { FenXiangService.PAY_KEY, PayKey }, //"b2f752964f904e6a9ad9397c3ded2e28"
                { FenXiangService.PAY_SECRET, Secret }//c37ae8ec310b4a68881ec49473d571a4
            };

            service.Init(para);
            var req = new FXRefundReq
            {
                refundAmount = refundAmount,//退款金额
                orderNo = orderNo,     //退款订单编号
                refundReason = refundResason
            };
            var rsp = service.Refund(req);
            var msg = rsp.returnCode;
            if ("SUCCESS".Equals(msg))
            {
                return true;
            }
            return false;




        }


    }
}
