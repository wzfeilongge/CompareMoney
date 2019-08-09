using CompareMoney.Common.Helper;
using CompareMoney.Core.Domain.Models;
using CompareMoney.IRepository;
using CompareMoney.IRepository.BaseRepository;
using CompareMoney.IServices;
using CompareMoney.Repository;
using CompareMoney.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.Services
{
    public class FXStmtLineServices : BaseServicesInforHis<FXStmtLine>, IFXStmtLineServices
    {
        public readonly IFXStmtLineRepository _fXStmtLineServices;

        public readonly IPayTableRepository _payTableRepository;
        public FXStmtLineServices(IFXStmtLineRepository IFXStmtLineServices, IPayTableRepository payTableRepository)
        {
            _fXStmtLineServices = IFXStmtLineServices;

            _payTableRepository = payTableRepository;

        }

        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="Billdate"></param>
        /// <returns></returns>
        public async Task<int> GetFXStmtLines(string[] Billdate)
        {
            var count = 0;
            PayTable p = null;
            for (var i = 0; i < Billdate.Length; i++)
            {
                var data = CommServices.GetOrderTime(Billdate[i]);
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        var model = await _fXStmtLineServices.GetModelAsync(obj => obj.bankTrxnNo == item.bankTrxnNo);
                        if (model != null) continue;
                        p = new PayTable
                        {
                            bankTrxnNo = item.bankTrxnNo,
                            orderNo = item.orderNo,
                            trxNo = item.trxNo,
                            payWayCode = item.payWayCode,
                            payWayName = item.payWayName
                        };
                        p.orderNo = item.orderNo;
                        p.orderTime = item.orderTime;
                        p.orderDate = item.orderDate;
                        p.orderAmount = item.orderAmount;
                        p.productName = item.productName;
                        p.isRefund = item.isRefund;
                        p.refundAmount = item.refundAmount;
                        await _payTableRepository.Add(p);                      
                            count++;                      
                    }
                }
            }
            return count;          
        }
    }
}
