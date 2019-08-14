using CompareMoney.Common.Helper;
using CompareMoney.Core.Domain.Models;
using CompareMoney.IRepository;
using CompareMoney.IServices;
using CompareMoney.Services.Base;
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

        public UserServices(IUserRepository IUserRepository, IOutMoneyRepository IMoneyServices)
        {
            _userServices = IUserRepository;
            _moneyServices = IMoneyServices;
        }

        public async Task<User> Login(string name, string Password)
        {
            var model = await _userServices.GetModelAsync(u => u.UserName == name && u.Password == Password);
            if (model != null)
            {
                //  myLogger.Info("登录成功");
                return (model);
            }
            return null;
            //  throw new NotImplementedException();
        }
       // ArrayList a = new ArrayList();      
        public async Task<bool> OutMoney(string AdminPassword, string orderNo, string refundReason, string refundAmount)
        {       
            var model = await _userServices.GetModelAsync(u => u.AdminPassword == AdminPassword);
            // myLogger.Info("用户开始请求退费");
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
                    var count =  await _moneyServices.AddModel(MoneyModel);
                    if (count == 1)
                    {
                              return true;
                          }
                      //    myLogger.Error("退费完成但是数据库写入失败");
                        return false;
                    }
                }
            return false;
            //  myLogger.Error("用户尝试退费，但是验证失败");
        }
           
        }
    }

