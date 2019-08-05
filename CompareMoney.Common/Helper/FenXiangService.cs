using CompareMoney.Core.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace CompareMoney.Common.Helper
{
    public class FenXiangService
    {
        const string SUCCESS = "SUCCESS";
        const string FAIL = "FAIL";

        // alipay
        public const string PAY_INIT_URL = "pay_init_url";
        public const string LIVE_INIT_URL = "live_init_url";
        public const string PAY_URL = "pay_url";
        public const string REFUND_URL = "refund_url";
        public const string CANCEL_URL = "cancel_url";
        public const string AUTH_QRY_URL = "auth_qry_url";
        // weixin
        public const string WX_AUTH_INFO = "wx_auth_info";
        public const string WX_FACE_PAY = "wx_face_pay";
        //
        public const string PAY_KEY = "pay_key";
        public const string PAY_SECRET = "pay_secret";
        // statement
        public const string STMT_QRY_URL = "stmt_qry_url";

        string _urlPayInit;
        string _urlLiveInit;
        string _urlPay;
        string _urlRefund;
        string _urlCancel;
        string _urlAuthQry;
        string _urlWxAuthInfo;
        string _urlWxFacePay;
        string _payKey;
        string _paySecret;
        string _urlStmtQry;

        public void Init(Dictionary<string, string> paras)
        {
            paras.TryGetValue(PAY_INIT_URL, out _urlPayInit);
            paras.TryGetValue(LIVE_INIT_URL, out _urlLiveInit);
            paras.TryGetValue(PAY_URL, out _urlPay);
            paras.TryGetValue(REFUND_URL, out _urlRefund);
            paras.TryGetValue(CANCEL_URL, out _urlCancel);
            paras.TryGetValue(AUTH_QRY_URL, out _urlAuthQry);
            paras.TryGetValue(PAY_KEY, out _payKey);
            paras.TryGetValue(PAY_SECRET, out _paySecret);
            paras.TryGetValue(WX_AUTH_INFO, out _urlWxAuthInfo);
            paras.TryGetValue(WX_FACE_PAY, out _urlWxFacePay);
            paras.TryGetValue(STMT_QRY_URL, out _urlStmtQry);
        }

        protected TRsp DoTran<TReq, TRsp>(string url, TReq req) where TReq : FXModelReq where TRsp : FXModelRsp
        {
            return DoTran<TReq, TRsp>(url, req, true);
        }

        protected TRsp DoTran<TReq, TRsp>(string url, TReq req, bool verify)
            where TReq : FXModelReq
            where TRsp : FXModelRsp
        {
            FXComm comm = new FXComm();
            req.payKey = _payKey;
            req.sign = FXComm.Sign(req, _paySecret);
            string jsonreq = JsonConvert.SerializeObject(req);

            string jsonrsp = comm.PostReq(url, jsonreq);

            if (verify)
                FXComm.Verify(jsonrsp, _paySecret);
            TRsp rsp = JsonConvert.DeserializeObject<TRsp>(jsonrsp);
            if (rsp.returnCode != SUCCESS)
            {
                throw new Exception(rsp.returnMsg);
            }
            return rsp;
        }

        public FXInitRsp Init(FXInitReq req)
        {
            return DoTran<FXInitReq, FXInitRsp>(_urlPayInit, req);
        }

        public FXInitRsp LiveInit(FXInitReq req)
        {
            return DoTran<FXInitReq, FXInitRsp>(_urlLiveInit, req);
        }

        public FXDoPayRsp DoPay(FXDoPayReq req)
        {
            return DoTran<FXDoPayReq, FXDoPayRsp>(_urlPay, req);
        }

        public FXAuthQryRsp AuthQry(FXAuthQryReq req)
        {
            return DoTran<FXAuthQryReq, FXAuthQryRsp>(_urlAuthQry, req);
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public FXRefundRsp Refund(FXRefundReq req)
        {
            return DoTran<FXRefundReq, FXRefundRsp>(_urlRefund, req);
        }

        // 微信

        public FXWXAuthInfoRsp WxAuthInfo(FXWXAuthInfoReq req)
        {
            return DoTran<FXWXAuthInfoReq, FXWXAuthInfoRsp>(_urlWxAuthInfo, req);
        }

        public FXWXFacePayRsp WxFacePay(FXWXFacePayReq req)
        {
            return DoTran<FXWXFacePayReq, FXWXFacePayRsp>(_urlWxFacePay, req);
        }

        // 对账

        public FXStmtQryRsp QryStatement(FXStmtQryReq req)
        {
            return DoTran<FXStmtQryReq, FXStmtQryRsp>(_urlStmtQry, req, false);
        }

    }


    public class FXComm
    {
        public string PostReq(string url, string reqtxt)
        {
            byte[] cont = Encoding.UTF8.GetBytes(reqtxt);
            HttpWebRequest webreq = WebRequest.Create(url) as HttpWebRequest;
            webreq.Method = "POST";
            //webreq.Accept = "application/json, charset=UTF-8";
            webreq.ContentType = "application/json; charset=UTF-8";
            using (Stream stream1 = webreq.GetRequestStream())
            {
                stream1.Write(cont, 0, cont.Length);
                stream1.Close();
            }
            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)webreq.GetResponse();
            }
            catch (WebException ex)
            {
                rsp = ex.Response as HttpWebResponse;
                if (rsp == null)
                    throw;
                string txt = null;
                using (Stream stream2 = rsp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream2);
                    txt = reader.ReadToEnd();
                    reader.Close();
                    stream2.Close();
                }
                if (string.IsNullOrEmpty(txt))
                    throw;
                else
                    throw new Exception(txt, ex);
            }
            if (rsp == null)
                throw new Exception("返回内容为null");
            string rsptxt = null;
            using (Stream stream3 = rsp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream3);
                rsptxt = reader.ReadToEnd();
                reader.Close();
                stream3.Close();
            }
            return rsptxt;
        }

        public static void Verify(string jsonstr, string seckey)
        {
            SortedDictionary<string, string> tmp = JsonConvert.DeserializeObject<SortedDictionary<string, string>>(jsonstr);
            if (!tmp.ContainsKey("sign"))
                return;
            string sign = (string)tmp["sign"];
            tmp.Remove("sign");
            StringBuilder sb = new StringBuilder();
            bool bfirst = true;
            foreach (KeyValuePair<string, string> pair in tmp)
            {
                if (string.IsNullOrEmpty(pair.Value))
                    continue;
                if (bfirst)
                    bfirst = false;
                else
                    sb.Append("&");
                sb.Append(pair.Key);
                sb.Append("=");
                sb.Append(pair.Value.ToString());
            }
            string text = sb.ToString() + "&paySecret=" + seckey;
            using (MD5 md5 = MD5.Create())
            {
                byte[] temp = Encoding.UTF8.GetBytes(text);
                md5.TransformFinalBlock(temp, 0, temp.Length);
                string hash = ByteArrayToHex(md5.Hash);
                if (hash != sign)
                    return;
            }
        }

        public static string Sign(object obj, string seckey)
        {
            Type type = obj.GetType();
            FieldInfo[] info = type.GetFields();
            List<FieldInfo> linfo = new List<FieldInfo>();
            linfo.AddRange(info);
            linfo.Sort((info1, info2) => { return String.Compare(info1.Name, info2.Name, StringComparison.Ordinal); });
            bool bfirst = true;
            StringBuilder sb = new StringBuilder();
            foreach (FieldInfo fi in linfo)
            {
                if (fi.Name == "sign")
                    continue;
                object val = fi.GetValue(obj);
                if (val == null)
                    continue;
                string strval = val.ToString();
                if (string.IsNullOrEmpty(strval))
                    continue;
                if (bfirst)
                    bfirst = false;
                else
                    sb.Append("&");
                sb.Append(fi.Name);
                sb.Append("=");
                sb.Append(strval);
            }
            string text = sb.ToString() + "&paySecret=" + seckey;
            using (MD5 md5 = MD5.Create())
            {
                byte[] temp = Encoding.UTF8.GetBytes(text);
                md5.TransformFinalBlock(temp, 0, temp.Length);
                string hash = ByteArrayToHex(md5.Hash);
                return hash;
            }
        }

        public static string ByteArrayToHex(byte[] val)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in val)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

    }
}
