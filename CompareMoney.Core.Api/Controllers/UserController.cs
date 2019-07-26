using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CompareMoney.Core.Api.ControllersModels;
using CompareMoney.Core.Domain.Models;
using CompareMoney.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Profiling;

namespace CompareMoney.Core.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {


        private readonly IUserServices _userServices;
  
        public UserController(IUserServices userServices, IConfiguration configuration)
        {

            _userServices = userServices;
           

        }


 





        /// <summary>
        /// 登陆获取model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("Login", Name = ("Login"))]

        //  [Authorize(Policy = "SystemOrAdmin")]

      //  [Authorize]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            var result = await _userServices.Login(request.UserName, request.PassWord);

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

        //  [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> OutMoney([FromBody] OutMoneyModel request)
        {

            // var result = await _userServices.GetModelAsync(u => u.AdminPassword == request.AdminPassword);
            var results = await _userServices.OutMoney(request.AdminPassword, request.OrderNo, request.RefundReason, request.RefundAmount);
            if (results)
            {

                return Ok(new SucessModel());



            }

            return Ok(new JsonFailCatch("退费失败"));
        }


        [HttpGet("test", Name = ("test"))]

        // [Authorize(Policy = "SystemOrAdmin")]
        public IActionResult Test()
        {

            return Ok(new JsonFailCatch("退费失败"));

        }












    }





}
