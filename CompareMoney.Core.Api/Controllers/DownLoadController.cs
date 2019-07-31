using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompareMoney.Core.Api.ControllersModels;
using CompareMoney.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompareMoney.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownLoadController : ControllerBase
    {

        public readonly DownLoadInterface _downLoadInterface;
        public DownLoadController(DownLoadInterface downLoadInterface)
        {
            _downLoadInterface = downLoadInterface;
        }



        /// <summary>
        /// 下载第三方数据到我方数据库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("DwonLoadTosql", Name = "DwonLoadTosql")]
       public async Task<IActionResult> DwonLoadTosql([FromBody]BillDataModel request) {

            var Count = await _downLoadInterface.GetFXStmtLines(request.BillDate);


            return Ok(new SucessModelCount(Count));

        }







    }
}
