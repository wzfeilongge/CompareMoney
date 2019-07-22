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

        [HttpGet("{Login}", Name = ("Login"))]
     

        public  async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            using (MiniProfiler.Current.Step("用户正在请求第登陆"))
            {
                

                    var result = await _userServices.Login(request.UserName, request.Password);


                    if (result!=null)
                    {


                        return Ok(new SucessModelData<User>(result));
                    }

                    MiniProfiler.Current.CustomTiming("Errors：", "请求登陆接口成功，但是找不到用户名或者密码");


                    return Ok(new JsonFailCatch("查询失败1"));

                
               


            }





            //  var result = ;
               
           
        }




        /// <summary>
        /// 退费的接口 返回true 或者false 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
         [HttpPost("{OutMoney}", Name = ("OutMoney"))]
        

        public async Task<IActionResult> OutMoney([FromBody] OutMoneyModel request)
        {
            using (MiniProfiler.Current.Step("用户正在请求退费"))
            {
                try
                {
                   // var result = await _userServices.GetModelAsync(u => u.AdminPassword == request.AdminPassword);
                    var results = await _userServices.OutMoney(request.AdminPassword,request.orderNo,request.refundReason,request.refundAmount);
                    if (results ==true)
                    {
                       // MiniProfiler.Current.CustomTiming("Errors：", "退费失败");
                        return Ok(new SucessModel());

                        //return Ok(new SucessModelData<User>(result));

                    }

                    return Ok(new JsonFailCatch("退费失败"));
                }
                catch (Exception e)
                {
                    MiniProfiler.Current.CustomTiming("Errors：", e.Message);

                    return Ok(new JsonFailCatch("退费失败"));
                }


            }

          







        }




    }
}
