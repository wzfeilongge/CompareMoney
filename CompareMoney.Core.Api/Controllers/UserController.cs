using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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
using NIO.VI.Jobs.Tools;
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
            var result = await _userServices.Login(request.UserName, request.PassWord);
            if (result != null)
            {
                TokenModelJwt t = new TokenModelJwt
                {
                    Role = result.Role,
                    Uid = result.Id,
                    Name = result.UserName
                };
                string token = JwtHelpers.IssueJwt(t);
                if (token != null)
                {
                    result.token = token;
                    result.Password = "******";
                    result.AdminPassword = "******";
                }
                return Ok(new SucessModelData<User>(result));
            }
            return Ok(new JsonFailCatch("登录失败"));
        }

        /// <summary>
        /// 测试用 需要先登录获取token
        /// </summary>
        /// <returns></returns>
        [HttpPost("test", Name = ("test"))]
        [Authorize(Policy = "SystemOrAdmin")]
        [Authorize(Policy = "Guest")]
        public IActionResult Test()
        {
            return Ok(new JsonFailCatch("Code其实200,Token测试也是成功的"));
        }

        /// <summary>
        /// 退费的接口 返回true 或者false  需要先登录获取token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutMoney", Name = ("OutMoney"))]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> OutMoney([FromBody] OutMoneyModel request)
        {
            var results = await _userServices.OutMoney(request.AdminPassword, request.OrderNo, request.RefundReason, request.RefundAmount);
            if (results)
            {
                return Ok(new SucessModel());
            }
            return Ok(new JsonFailCatch("退费失败"));
        }

        /// <summary>
        /// 发送邮件 需要先登录获取token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SendMail", Name = ("SendMail"))]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> SendMail([FromBody] SendMailModel request)
        {
            string FileName = "postman_collection.json";
            var stream = MailHelp.FileToStream(FileName);
            Dictionary<Stream, string> IoFile = new Dictionary<Stream, string>
            {
                { stream, FileName }
            };
            await MailHelp.SendMailAsync(request.Smtpserver, request.UserName, request.Pwd, request.ToMail, request.Subj, request.Bodys, request.FromMail, attachments: IoFile);
            return Ok(new SucessModel());
        }
    }
}
