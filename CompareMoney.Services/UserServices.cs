using CompareMoney.Common.Helper;
using CompareMoney.Core.Domain.Models;
using CompareMoney.IRepository;
using CompareMoney.IServices;
using CompareMoney.Services.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.Services
{
    public class UserServices : BaseServicesInforPay<User>, IUserServices
    {
        private readonly IUserRepository _userServices;
        private readonly IOutMoneyRepository _moneyServices;
        private readonly ILogger<UserServices> _myLogger;
        public UserServices(IUserRepository IUserRepository, IOutMoneyRepository IMoneyServices, ILogger<UserServices> myLogger)
        {
            _userServices = IUserRepository;
            _moneyServices = IMoneyServices;
            _myLogger = myLogger;
        }

        public async Task<User> Login(string name, string Password)
        {
            var model = await _userServices.GetModelAsync(u => u.UserName == name && u.Password == Password);
            if (model != null)
            {
                _myLogger.LogInformation("登录成功");
                return (model);
            }
            return null;
            //  throw new NotImplementedException();
        }
        // ArrayList a = new ArrayList();      
        public async Task<bool> OutMoney(string AdminPassword, string orderNo, string refundReason, string refundAmount)
        {
            var model = await _userServices.GetModelAsync(u => u.AdminPassword == AdminPassword);
            _myLogger.LogInformation("用户开始请求退费");
            if (model != null)
            {
                var flag = CommServices.Refund(refundAmount, orderNo, refundReason);
                if (flag)
                {
                    var MoneyModel = new OutMoneyTable();
                    MoneyModel.now = DateTime.UtcNow;
                    MoneyModel.orderNo = orderNo;
                    MoneyModel.refundAmount = refundAmount;
                    MoneyModel.refundReason = refundReason;
                    var count = await _moneyServices.AddModel(MoneyModel);
                    if (count == 1)
                    {
                        return true;
                    }
                    _myLogger.LogError("退费完成但是数据库写入失败");
                    return false;
                }
            }
            return false;

        }

    }
}

