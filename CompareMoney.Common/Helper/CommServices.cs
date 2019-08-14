using CompareMoney.Core.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompareMoney.Common.Helper
{
  public  class CommServices
    {
        /// <summary>
        /// 退费的接口
        /// </summary>
        /// <param name="refundAmount"></param>
        /// <param name="orderNo"></param>
        /// <param name="refundResason"></param>
        /// <param name="PayKey"></param>
        /// <param name="Secret"></param>
        /// <returns></returns>
        public static bool Refund(string refundAmount, string orderNo, string refundResason)
        {
            FenXiangService service = new FenXiangService();
            Dictionary<string, string> para = new Dictionary<string, string>
            {
                { FenXiangService.REFUND_URL, dopay },
                { FenXiangService.PAY_KEY, paykey }, //"b2f752964f904e6a9ad9397c3ded2e28"
                { FenXiangService.PAY_SECRET, realsec }//c37ae8ec310b4a68881ec49473d571a4
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

        /// <summary>
        /// 根据日期下载数据
        /// </summary>
        /// <param name="billDate"></param>
        /// <returns></returns>
        public static FXStmtLine[] GetOrderTime(string billDate)
        {
            InitConfiguration();
            FenXiangService service = new FenXiangService();
            Dictionary<string, string> para = new Dictionary<string, string>
            {
                { FenXiangService.STMT_QRY_URL, downLoad },
                { FenXiangService.PAY_KEY, paykey }, //"b2f752964f904e6a9ad9397c3ded2e28"
                { FenXiangService.PAY_SECRET, realsec }//c37ae8ec310b4a68881ec49473d571a4
            };
            service.Init(para);
            FXStmtQryReq req = new FXStmtQryReq
            {
                billDate = billDate
            };
            FXStmtQryRsp rsp = service.QryStatement(req);
            FXStmtLine[] data = rsp.data;
            return data;
        }


        public static string init { get; set; }

        public static string dopay { get; set; }

        public static string refund { get; set; }

        public static string merchantId { get; set; }

        public static string partnerId { get; set; }

        public static string appId { get; set; }

        public static string paykey { get; set; }

        public static string paysec { get; set; }

        public static string downLoad { get; set; }

        public static string realsec { get; set; }

        public static string istrue { get; set; }

        #region 初始化Configuration
        /// <summary>
        /// 初始化Configuration
        /// </summary>
        public static void InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();
            //myLogger.Info("初始化配置文件");
            init = config["BaseSet:init"]; 
            dopay = config["BaseSet:dopay"]; ;
            refund = config["BaseSet:refund"]; ;
            merchantId = config["BaseSet:merchantId"]; ;
            partnerId = config["BaseSet:partnerId"]; ;
            appId = config["BaseSet:appId"]; ;
            paysec = config["BaseSet:paysec"]; ;
            paykey = config["BaseSet:paykey"]; ;
            downLoad = config["BaseSet:downLoad"]; ;
            istrue = config["BaseSet:istrue"]; ;
            if ("true".Equals(istrue))
            {
                Console.WriteLine("sec有加密");              
                realsec = FacePayEncrypt.Decrypt(paysec);
            }
            else
            {
                Console.WriteLine("sec无加密");
                //  myLogger.Warn("sec无加密");
                realsec = paysec;
            }
        }
        #endregion

    }
}
