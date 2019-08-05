using CompareMoney.Core.Domain.Models;
using IServices.BaseServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.IServices
{
    public interface IUserServices : IBaseEntityInforPay<User>
    {
        Task<User> Login(string name, string Password);

        Task<bool> OutMoney(string AdminPassword, string orderNo, string refundReason, string refundAmount);
    }
}
