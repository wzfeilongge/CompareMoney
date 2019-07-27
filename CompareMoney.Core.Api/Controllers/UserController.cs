using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CompareMoney.Core.Api.ControllersModels;
using CompareMoney.Core.Api.JwtHelper;
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
        /// 登陆获取Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("Login", Name = ("Login"))]


        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            string token = string.Empty;

            var result = await _userServices.Login(request.UserName, request.PassWord);

            if (result != null)
            {
                TokenModelJwt t = new TokenModelJwt
                {
                    Role = result.Role,
                    Uid = result.Id,
                    Name = result.UserName
                };
                token = JwtHelpers.IssueJwt(t);

                return Ok(new SucessModelData<string>(token));

            }

            return Ok(new JsonFailCatch(token));
        }



        [HttpPost("test", Name = ("test"))]
        [Authorize(Roles = "Admin")]
        public IActionResult Test()
        {

            return Ok(new JsonFailCatch("其实是成功的"));

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
















    }





}
