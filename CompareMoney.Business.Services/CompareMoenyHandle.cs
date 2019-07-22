﻿
using CompareMoney.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using  CompareMoney.Services;
using CompareMoney.Applicaion.ViewModel;
using System.Linq;
using CompareMoney.IRepository;

namespace CompareMoney.Business.Services.Domain
{
    public class CompareMoenyHandle : CompareMoneyInterface
    {
      

        private readonly IPayTableRepository _ipayTableServices;

        

        private readonly IVIEW_JYMXTableRepository _iJYMXTableServices;


        public CompareMoenyHandle(IPayTableRepository ipayTableServices,

            IVIEW_JYMXTableRepository iJYMXTableServices)
        {

         


            _ipayTableServices = ipayTableServices;


        


            _iJYMXTableServices = iJYMXTableServices;


        }






        /// <summary>
        /// 根据时间获取Pay的数据
        /// </summary>
        /// <param name="BillDate"></param>
        /// <returns></returns>
        public async Task<List<DetailedList>> DetailedListInPayOrder(string[] BillDate)
        {

            var datas = new List<DetailedList>();


            for (var i = 0; i < BillDate.Length; i++)
            {


                var PayModel = await _ipayTableServices.Query(obj => obj.orderDate == BillDate[i]);


                if (PayModel.Count == 0)
                {  //查询时间数组中 那天的数据 空表示没有数据 进行时间数组中下一天的查询
                   // myLogger.Warn($"{BillDate[i]} 无数据");
                    continue;
                }
                decimal returnMoney = 0;
                decimal moneys = 0;
                int count = 0;

                foreach (var pay in PayModel) //查询时间数组中 那天的数据 如果有数据
                {
                    if ("YES".Equals(pay.isRefund))
                    {
                        returnMoney += Convert.ToDecimal(pay.orderAmount);


                    }
                    moneys += Convert.ToDecimal(pay.orderAmount); //类型不相同需要强转
                    count++;

                }

                if (count == 0)
                {
                    return datas;
                }
                var dt = new DetailedList
                {
                    Count = count,
                    BillDate = BillDate[i],
                    Money = Convert.ToDouble(moneys - returnMoney),
                    DataName = "Pay"
                };
                datas.Add(dt);
            }

            return datas;

        }


        /// <summary>
        /// 根据时间获取His的数据
        /// </summary>
        /// <param name="BillDate"></param>
        /// <returns></returns>
        public async Task<DetailedList> DetailedListInHisOrday(string BillDate)
        {

            double moneys = 0.00;
            int count = 0;


            var HisModel = await _iJYMXTableServices.Query(obj => obj.BILLDATE == BillDate); //根据时间查询某天的数据
            if (HisModel.Count == 0)
            {
                //myLogger.Warn($"{BillDate}  无数据");
                return null;
            }
            //   myLogger.Info("正在处理金额和总数量");
            foreach (var his in HisModel) //查询时间数组中 那天的数据 如果有数据
            {
                moneys += (his.TRADEMONEY); //类型不相同需要强转
                count++;

            }

            var dt = new DetailedList
            {
                Count = count,
                BillDate = BillDate,
                Money = moneys,
                DataName = "HIS"
            };


            return dt;
        }

        /// <summary>
        /// 第一层比对返回结果
        /// </summary>
        /// <param name="BillDate"></param>
        /// <returns></returns>
        public async Task<List<poolModel>> DetailedList(string[] BillDate)
        {
            List<poolModel> list = new List<poolModel>();
            var datas = await DetailedListInPayOrder(BillDate);

            foreach (var item in datas)
            {

                poolModel p = new poolModel
                {
                    BillDate = item.BillDate
                };




                var hismodel = await DetailedListInHisOrday(item.BillDate);
                if (hismodel != null)
                {
                    //myLogger.Info($"{item.BillDate} 当日有数据");
                    p.HisCount = hismodel.Count;
                    p.HisMoney = hismodel.Money.ToString();
                    p.errorCount = item.Count - p.HisCount;
                    p.PayMoney = item.Money.ToString("0.00");
                    if (p.errorCount != 0)
                    {

                        p.errorMoney = string.Format("{0:F}", Math.Abs(item.Money - hismodel.Money));

                        // myLogger.Info($"{p.BillDate}  {p.PayMoney} 第三方金额");
                        //    myLogger.Info($"{p.BillDate}  {p.errorMoney} 错误金额");

                    }
                    else
                    {
                        p.errorMoney = (p.errorCount).ToString();
                        //    myLogger.Info($"{p.BillDate}  {p.PayMoney} 第三方金额");
                        //    myLogger.Info($"{p.BillDate}  {p.errorMoney} 错误金额");


                    }
                }
                else
                {
                    //    myLogger.Warn($"{item.BillDate} 当日无数据");
                    p.HisMoney = "0.00";
                    p.PayMoney = item.Money.ToString("0.00");
                    p.errorMoney = item.Money.ToString("0.00");
                    p.errorCount = item.Count;
                    p.HisCount = 0;

                    //  myLogger.Info($"{p.BillDate}  {p.PayMoney} 第三方金额");
                    //   myLogger.Info($"{p.BillDate}  {p.errorMoney} 错误金额");


                }

                p.PayCount = item.Count;
                if (item.Count != p.HisCount || p.HisMoney != p.PayMoney)
                {
                    p.isTrue = 0;

                }
                else
                {

                    p.isTrue = 1;
                }


                list.Add(p);
            }












            throw new NotImplementedException();





        }


        /// <summary>
        /// 第二层比对
        /// </summary>
        /// <param name="BillDate"></param>
        /// <returns></returns>
        public async Task<List<CompareData>> DetailedListAll(string[] BillDate)
        {
            List<CompareData> datas = new List<CompareData>();
            for (var i = 0; i < BillDate.Length; i++)
            {
                var queue = await _ipayTableServices.Query(e => e.orderDate == BillDate[i]);
                if (queue == null)
                {
                    CompareData dt = new CompareData
                    {
                        BillDate = BillDate[i],
                        isTrue = 0
                    };
                    datas.Add(dt);
                    continue;
                }

                foreach (var Pay in queue)
                {
                    // myLogger.Info("开始比对数据");
                    var paymones = Convert.ToDouble((Pay.orderAmount));
                    //    myLogger.Info($"强转Pay的金额数据{paymones}");
                    var Hislists = await _iJYMXTableServices.GetModelAsync(obj => obj.BILLDATE == BillDate[i] && obj.THIRDTRANSACTIONID == Pay.bankTrxnNo && obj.PAYTRANSACTIONID == Pay.orderNo && obj.TRADEMONEY == paymones);
                    //   myLogger.Info($"日期-{BillDate[i]} 第三方Id-{Pay.bankTrxnNo} 支付流水-{Pay.orderNo} 病人卡号-{Pay.trxNo} 金额-{Pay.orderAmount} ");
                    string str = Pay.productName;
                    if (Hislists != null)
                    {
                        //   myLogger.Info($"开始比对{BillDate[i]}的数据");
                        if ("YES" == (Pay.isRefund)) //表示是否已经退过费用
                        {

                            //  myLogger.Info($"{BillDate[i]}有数据,pay有退费");

                        }

                        CompareData dts = new CompareData
                        {
                            BillDate = BillDate[i],
                            tradeTime = Pay.trxNo,
                            payTransTransactionId = Pay.orderNo,
                            tradeMoney = Pay.orderAmount,
                            hisTransactionId = Pay.orderNo,
                            pateintTradeType = ChangeType(Pay.payWayCode),
                            thirdTransactionId = Pay.bankTrxnNo,
                            patientName = Hislists.PATIENTNAME,
                            pateontID = Hislists.TRADEID.ToString(),
                            pateontCardNo = Hislists.PATIENTCARDNO,
                            IsRefund = Hislists.ISREFUND,
                            tradeComment = str,
                            operatorId = Hislists.OPERATORID,
                            transactionTime = Pay.orderTime,
                            orderNo = Pay.orderNo,
                            isTrue = 1  //数据正常
                        };
                        datas.Add(dts);

                    }
                    else
                    {

                        if ("YES".Equals(Pay.isRefund)) //表示是否已经退过费用
                        {
                            //myLogger.Info($"{BillDate[i]}His无数据,Pay有退费");

                            str = "已正常退费";
                            CompareData dt = new CompareData
                            {
                                IsRefund = 1,
                                tradeTime = Pay.orderTime,
                                BillDate = BillDate[i],
                                tradeMoney = Pay.orderAmount,
                                payTransTransactionId = Pay.orderNo,
                                hisTransactionId = Pay.orderNo,
                                pateintTradeType = ChangeType(Pay.payWayCode),
                                thirdTransactionId = Pay.bankTrxnNo,
                                transactionTime = Pay.orderTime,
                                tradeComment = str,
                                isTrue = 1, //数据异常
                                            //     EnabledMoney = "0"
                            };
                            datas.Add(dt);
                        }
                        else
                        {
                            //   var queues = DomainHis.GetModel(obj => obj.BILLDATE == BillDate[i]);
                            //   myLogger.Warn($"{BillDate[i]}His无数据,无退费");
                            CompareData dt = new CompareData
                            {
                                IsRefund = 0,
                                tradeTime = Pay.orderTime,
                                BillDate = BillDate[i],
                                tradeMoney = Pay.orderAmount,
                                payTransTransactionId = Pay.orderNo,
                                hisTransactionId = Pay.orderNo,
                                pateintTradeType = ChangeType(Pay.payWayCode),
                                thirdTransactionId = Pay.bankTrxnNo,
                                transactionTime = Pay.orderTime,
                                tradeComment = str,
                                isTrue = 0 //数据异常
                            };
                            datas.Add(dt);


                        }



                    }



                }



            }

            return datas;











        }




        public static int ChangeType(string type)
        {
            if (type.Equals("WEIXIN"))
            {
                return 2;
            }
            if (type.Equals("ALIPAY"))
            {

                return 1;
            }
            return 10;

        }

        public async Task<List<CompareData>> DetailedListError(string[] BillDate)
        {
            //List<CompareData> datas = new List<CompareData>();
            //for (var i = 0; i < BillDate.Length; i++)
            //{
            //    var queue =  await _ipayTableServices.Query(e => e.orderDate == BillDate[i]);
            //    if (queue == null)
            //    {
            //        CompareData dt = new CompareData
            //        {
            //            BillDate = BillDate[i],
            //            isTrue = 0
            //        };
            //        datas.Add(dt);
            //        continue;
            //    }
            //    foreach (var Pay in queue)
            //    {
            //        var Hislists = _iJYMXTableServices.GetModelAsync(obj => obj.BILLDATE == BillDate[i] && obj.HISTRANSACTIONID == Pay.bankTrxnNo && obj.PAYTRANSACTIONID == Pay.orderNo &&
            //        Pay.orderAmount == obj.TRADEMONEY.ToString());
            //        if (Hislists != null)
            //        {
            //            continue;
            //        }
            //        else
            //        {
            //            int flag = 0;
            //            string str = "";
            //            if ("YES".Equals(Pay.isRefund)) //表示是否已经退过费用
            //            {
            //                flag = 1;
            //                str = "已正常退费";
            //                continue;

            //            }
            //            CompareData dt = new CompareData
            //            {
            //                tradeTime = Pay.orderTime,
            //                BillDate = BillDate[i],
            //                tradeMoney = Pay.orderAmount,
            //                payTransTransactionId = Pay.trxNo,
            //                hisTransactionId = "",//Pay.bankTrxnNo,
            //                pateintTradeType = 0,
            //                thirdTransactionId = Pay.orderNo,
            //                transactionTime = Pay.orderTime,
            //                tradeComment = str,
            //                isTrue = flag //数据异常
            //            };
            //            datas.Add(dt);

            //        }



            //    }



            //}

          //  

          var lists = await DetailedListAll(BillDate);
          List<CompareData> list = lists.Where(obj => obj.isTrue ==2).ToList();


             return list;
            // return await DetailedListAll(BillDate);


            throw new NotImplementedException();
        }





        public  IEnumerable<CompareData> PapingList(int isTrue, List<CompareData> list, int pageNo, int pageSize)
        {

            IEnumerable<CompareData> trueData = null;
            if (isTrue == 1)
            {

                

                trueData=   list.Where(obj => obj.isTrue == 1).OrderBy(obj => obj.transactionTime).Skip((pageNo - 1) * pageSize).Take(pageSize).ToArray();

               return trueData;
               // var trueCount = list.Where(obj => obj.isTrue == 1).Count();
            }
            else
            {
                return list.OrderBy(obj => obj.transactionTime).Skip((pageNo - 1) * pageSize).Take(pageSize).ToArray();



            }



        }

       
    }




}
