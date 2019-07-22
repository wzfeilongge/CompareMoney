using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


//namespace CompareMoney.Domain.Models

namespace CompareMoney.Core.Domain.Models
{
    public class FXModelBase
    {
        public string sign;
    }

    public class FXModelReq : FXModelBase
    {
        public string payKey;
    }

    public class FXModelRsp : FXModelBase
    {
        public string returnCode;
        public string returnMsg;
    }

    public class FXInitReq : FXModelReq
    {
        public string metaInfo;
    }

    public class FXInitRsp : FXModelRsp
    {
        public string zimId;
        public string zimInitClientData;
    }

    public class FXDoPayReq : FXModelReq
    {
        public string authCode;
        public string productName;
        public string orderNo;
        public string orderPrice;
        public string payWayCode;
        public string orderIp;
        public string orderDate;
        public string orderTime;
        public string remark;
    }

    public class FXDoPayRsp : FXModelRsp
    {
        public string orderNo;
        public string productName;
        public string orderPrice;
        public string remark;
        public string trxNo;
        public string bankOrderNo;
        public string bankTrxnNo;
    }

    public class FXAuthQryReq : FXModelReq
    {
        public string authCode;
    }

    public class FXAlipayUserInfo
    {
        public Dictionary<string, string> alipay_user_info_share_response;
    }

    public class FXAuthQryRsp : FXModelRsp
    {
        public string userInfo;
    }

    public class FXRefundReq : FXModelReq
    {
        public string orderNo;
        public string refundAmount;
        public string refundReason;
    }

    public class FXRefundInfo
    {
        public string orderNo;
        public string trxNo;
        public string productName;
        public decimal refundAmount;
        public decimal orderAmount;
        public string completeTime;
    }

    public class FXRefundRsp : FXModelRsp
    {
        public string refundInfo;
    }

    public class FXWXAuthInfoReq : FXModelReq
    {
        public string storeId;
        public string storeName;
        public string deviceId;
        public string attach;
        public string rawData;
    }

    public class FXWXAuthInfoRsp : FXModelRsp
    {
        public string authInfo;
        public string expiresIn;
    }

    public class FXWXFacePayReq : FXModelReq
    {
        public string openId;
        public string authCode;
        public string productName;
        public string orderNo;
        public string orderPrice;
        public string payWayCode;
        public string orderIp;
        public string orderDate;
        public string orderTime;
        public string notifyUrl;
        public string remark;
    }

    public class FXWXFacePayRsp : FXModelRsp
    {
        public string orderNo;
        public string trxNo;
        public string bankOrderNo;
        public string bankTrxnNo;
        public string productName;
        public string orderPrice;
        public string remark;
    }

    public class FXStmtQryReq : FXModelReq
    {
        public string billDate;
    }

    public class FXStmtLine
    {
        [Key]
        public string bankTrxnNo { get; set; }
        public string orderNo { get; set; }
        public string trxNo { get; set; }

        public string payWayCode { get; set; }
        public string payWayName { get; set; }
        public string orderDate { get; set; }
        public string orderTime { get; set; }
        public string orderAmount { get; set; }
        public string productName { get; set; }
        public string isRefund { get; set; }
        public string refundAmount { get; set; }
    }

    public class FXStmtQryRsp : FXModelRsp
    {
        public string merchantNo;
        public string merchantName;
        public FXStmtLine[] data;
    }



}

