using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompareMoney.Core.Api.ControllersModels;
using CompareMoney.Core.Domain.Models;
using CompareMoney.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;

namespace CompareMoney.Core.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {


        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {

            _userServices = userServices;

        }




        /// <summary>
        /// 登陆获取model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("Login", Name = ("Login"))]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
           var result = await _userServices.Login(request.UserName, request.Password);

           if (result != null)
            {


                return Ok(new SucessModelData<User>(result));

            }

            return Ok(new JsonFailCatch("查询失败"));
      }


      /// <summary>
        /// 退费的接口 返回true 或者false 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutMoney", Name = ("OutMoney"))]


        public async Task<IActionResult> OutMoney([FromBody] OutMoneyModel request)
        {

            // var result = await _userServices.GetModelAsync(u => u.AdminPassword == request.AdminPassword);
            var results = await _userServices.OutMoney(request.AdminPassword, request.orderNo, request.refundReason, request.refundAmount);
            if (results)
            {

                return Ok(new SucessModel());



            }

            return Ok(new JsonFailCatch("退费失败"));
        }












    }





}
