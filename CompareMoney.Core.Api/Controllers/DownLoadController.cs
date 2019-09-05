using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompareMoney.Core.Api.ControllersModels;
using CompareMoney.IServices;
using CompareMoney.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompareMoney.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownLoadController : ControllerBase
    {
        
        public readonly IFXStmtLineServices _downLoadInterface;

        private readonly ILogger<DownLoadController> _iloger;
        public DownLoadController(IFXStmtLineServices downLoadInterface,ILogger<DownLoadController> iloger)
        {
            _downLoadInterface = downLoadInterface;
            _iloger = iloger;
        }

        /// <summary>
        /// 下载第三方数据到我方数据库 需要先登录获取token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("DwonLoadTosql", Name = "DwonLoadTosql")]
        [Authorize(Policy = "SystemOrAdmin")]
        [Authorize(Policy = "Guest")]
        public async Task<IActionResult> DwonLoadTosql([FromBody]BillDataModel request) {

            var hosts = HttpContext.Request.Host;
            _iloger.LogDebug($"{hosts.Host}正在请求SortArray 端口是 {hosts.Port},{hosts.Value}");
            var Count = await _downLoadInterface.GetFXStmtLines(request.BillDate);
            return Ok(new SucessModelCount(Count));

        }
    }
}
