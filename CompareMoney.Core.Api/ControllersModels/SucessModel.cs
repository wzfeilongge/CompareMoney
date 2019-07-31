using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareMoney.Core.Api.ControllersModels
{
    public class SucessModel
    {

        public int Code { get; set; } = 200;

        public string Msg { get; set; } = "操作成功";
    }

    public class GdTest {

        /// <summary>
        /// 00：表示成功，
        ///其他表示失败
        /// </summary>
        public string resultCode { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string resultMsg { get; set; }


    }




    public class SucessModelData<T> : SucessModel
    {
        public T Data { get; set; }

        public SucessModelData(T data)
        {
            this.Data = data;
        }
    }

    public class SucessModelCount : SucessModel
    {
        public int Count { get; set; } = 0;

        public SucessModelCount(int count)
        {
            this.Count = count;

            this.Msg = "操作成功,共增加" + count + "条数据";
        }
    }
    public class SuccessDataPages<T> : SucessModel
    {
        public int PageSize { get; set; }

        public int PageNo { get; set; }

        public int TotalPage { get; set; }

        public int TotalCount { get; set; }

        public T Data { get; set; }

        public SuccessDataPages(T data, int pageSize, int pageNo, int totalPage, int totalCount)
        {
            this.Data = data;
            this.PageSize = pageSize;
            this.PageNo = pageNo;
            this.TotalPage = totalPage;
            this.TotalCount = totalCount;
        }




    }
      public class JsonFailCatch : SucessModel
    {

        public JsonFailCatch(string msg)
        {
            this.Code = 500;
            this.Msg = msg;
        }
    }


     #region 广东测试
    public class hiFee
    {

        /// <summary>
        /// 缴费编号
        /// </summary>
        public string hiFeeNo { get; set; }

        /// <summary>
        /// /患者编号
        /// </summary>
        public string patientId { get; set; }

        /// <summary>
        /// 患者名称
        /// </summary>
        public string patientName { get; set; }

        /// <summary>
        /// 医生编号
        /// </summary>
        public string organdoctorId { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string doctorName { get; set; }

        /// <summary>
        /// 就诊科室名
        /// </summary>
        public string organName { get; set; }

        /// <summary>
        /// 门诊流水号
        /// </summary>
        public string serialNumber { get; set; }

        /// <summary>
        /// 就诊日期
        /// </summary>
        public DateTime visitDate { get; set; }

        /// <summary>
        /// 自费类型  社会基本医疗保险  职工医保 等以中文名存在
        /// </summary>
        public string settleType { get; set; }

        /// <summary>
        /// 结算金额
        /// </summary>
        public double settleAmount { get; set; }


        /// <summary>
        /// 自费金额
        /// </summary>
        public double patientAmount { get; set; }

        /// <summary>
        /// 缴费状态:1表示已缴费，0表示未缴费
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 打印状态:1表示已打印，0表示未打印
        /// </summary>

        public int isPrint { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime createTime { get; set; }

        /// <summary>
        /// 医保传入信息（医保传入的处方xml）
        /// </summary>
        public string medicareInfo { get; set; }

        /// <summary>
        /// /诊断信息维护
        /// </summary>
        public string diagInfo { get; set; }



        /// <summary>
        /// 0非医保
        ///1市医保 
        ///2其他医保
        /// </summary>
        public int medicareType { get; set; }

        /// <summary>
        /// 挂号编号
        /// </summary>

        public string reservation { get; set; }
        /// <summary>
        /// 医疗人群
        ///1.职工2.居民3.离休
        /// </summary>

        public string medicalpulation { get; set; }

        /// <summary>
        /// 就诊类别
        /// </summary>
        public string organtype { get; set; }


        /// <summary>
        /// 一般诊疗计费
        ///0.否，1是
        /// </summary>
        public string diagnosis { get; set; }
        /// <summary>
        /// 结算标志
        ///  默认为1.出院结算
        ///  2中途结算
        /// 3预结算
        /// </summary>

        public string dyjslb { get; set; }

        /// <summary>
        /// Icd编码
        /// 对应人社的编码
        /// </summary>
        public string icd { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string diseasecode { get; set; }
        /// <summary>
        /// 病种代码
        ///（就诊类别为271时不可为空）
        ///对应人社的代码
        /// </summary>
        public string specific { get; set; }


        /// <summary>
        /// 缴费明细
        /// </summary>
        public List<HiFeeItem> hiFeeItem { get; set; }

    }

    public class HiFeeItem
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string feeItemName { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string feeItemAmount { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int feeItemNum { get; set; }
        /// <summary>
        /// 单位：如 包 、盒等
        /// </summary>
        public int feeItemUnit { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public double feeItemAllAmount { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string feeItemStandard { get; set; }

    }
    #endregion

    public class JsonQuery : GdTest {



      //  public string hiFee = "";

        public JsonQuery(hiFee hiFee)
        {
            this.resultCode = "200";
            this.resultMsg = JsonConvert.SerializeObject(hiFee); 
        }
    }



    public class JsonQueryPay : GdTest {

        public string resultmessage = "";
        public JsonQueryPay(resultmessageInfordopayformzsf resultmessages)
        {
            
            resultmessage = JsonConvert.SerializeObject(resultmessages);

        }

    }


    /// <summary>
    /// 支付聚合
    /// </summary>
    public class resultmessageInfordopayformzsf
    {

        /// <summary>
        /// 缴费编号
        /// </summary>
        public string hiFeeNo { get; set; }

        /// <summary>
        /// HIS收费(结算)流水号
        /// </summary>
        public long hisReceiptSn { get; set; }
        /// <summary>
        /// 发票金额
        /// </summary>
        public decimal receiptAmount { get; set; }
        /// <summary>
        /// 自付金额
        /// </summary>
        public decimal receiptCashAmount { get; set; }
        /// <summary>
        /// 统筹金额
        /// </summary>
        public decimal receiptOverAmount { get; set; }
        /// <summary>
        /// 发药窗口
        /// </summary>
        public string dispensaryWin { get; set; }
        /// <summary>
        /// 执行科室
        /// </summary>
        public string execDept { get; set; }
        /// <summary>
        /// 开单科室
        /// </summary>
        public string applyDept { get; set; }
        /// <summary>
        /// 发票分类码
        /// </summary>
        public string billCode { get; set; }
        /// <summary>
        /// 发票分类名称
        /// </summary>
        public string billName { get; set; }
        /// <summary>
        /// 发票分类金额
        /// </summary>
        public decimal billAmount { get; set; }


    }



}
