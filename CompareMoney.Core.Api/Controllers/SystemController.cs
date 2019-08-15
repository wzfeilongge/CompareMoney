using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompareMoney.Applicaion.ViewModel;
using CompareMoney.Core.Api.ControllersModels;
using CompareMoney.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;

namespace CompareMoney.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly ICompareMoneyInterface _compareMoneyInterface;


        public SystemController(ICompareMoneyInterface compareMoneyInterface)
        {

            _compareMoneyInterface = compareMoneyInterface;

        }

        /// <summary>
        /// 根据时间获取pay和His的非明细的数据 需要先登录获取token
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpGet("GetBillPage", Name = "GetBillPage")]
        [Authorize(Policy = "SystemOrAdmin")]
        [Authorize(Policy = "Guest")]
        public async Task<IActionResult> GetBillPage([FromBody] GetBillPageModel requestModel)
        {
            if (requestModel.BillDate.Length > 10)
            {

                return Ok(new JsonFailCatch("查询的数据不能超过10天"));

            }
            else if (requestModel.BillDate.Length == 0)
            {
                return Ok(new object());

            }

            var result = await _compareMoneyInterface.DetailedList(requestModel.BillDate); //获取Pay 和His非明细的数据
         
            #region 分页非明细的数据
            var Count = result.Count;
           

           var counts= (int)Math.Ceiling((decimal)Count / requestModel.PageSize);
            #endregion

            return Ok(new SuccessDataPages<List<poolModel>>(result, requestModel.PageSize, requestModel.PageNo, counts, Count));




        }




        /// <summary>
        /// 根据时间获取明细的数据 需要先登录获取token
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>

        [HttpGet("GetALLDataPage", Name = "GetALLDataPage")]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> GetALLDataPage([FromBody] GetALLDataPageModel requestModel)
        {
            if (requestModel.BillDate.Length > 10)
            {

                return Ok(new JsonFailCatch("最多只能查询10天"));

            }

            else if (requestModel.BillDate.Length == 0)
            {
                return Ok(new object());

            }

            #region 返回异常的数据
            if (requestModel.IsTrue == 0)
            {
                var errorList = await _compareMoneyInterface.DetailedListError(requestModel.BillDate);
                var Data = errorList.Where(obj => obj.isTrue == 0).OrderBy(obj => obj.transactionTime).Skip((requestModel.PageNo - 1) * requestModel.PageSize).Take(requestModel.PageSize).ToList();
                var falseCount = Data.Count();
                var errCounts = Data.Count;
                var Icounts = (int)Math.Ceiling((decimal)errCounts / requestModel.PageSize);

                return Ok(new SuccessDataPages<IEnumerable<CompareData>>(Data, requestModel.PageSize, requestModel.PageNo, Icounts, falseCount));
            } //返回异常的数据
            #endregion

            #region 返回正常的数据和全部的数据
            var result = await _compareMoneyInterface.DetailedListAll(requestModel.BillDate);
            #endregion

            #region 分页返回出来的数据
            var Count = result.Count;
            if (Count < 1)
            {
                Count = 1;
            }
            

            var counts = (int)Math.Ceiling((decimal)Count / requestModel.PageSize);
            #endregion

            #region 逻辑处理
            IEnumerable<CompareData> trueData = null;


            if (requestModel.IsTrue == 1) //如果是要正常的
            {
                trueData = result.Where(obj => obj.isTrue == 1).OrderBy(obj => obj.transactionTime).Skip((requestModel.PageNo - 1) * requestModel.PageSize).Take(requestModel.PageSize).ToArray();

                var trueCount = result.Where(obj => obj.isTrue == 1).Count();

                return Ok(new SuccessDataPages<IEnumerable<CompareData>>(trueData, requestModel.PageSize, requestModel.PageNo, counts, trueCount));

            }
            else   //否则返回全部
            {
               // result = result.OrderBy(obj = >obj.BillDate);
               var allData= result.OrderBy(obj => obj.transactionTime).Skip((requestModel.PageNo - 1) * requestModel.PageSize).Take(requestModel.PageSize).ToArray(); 

                return Ok(new SuccessDataPages<IEnumerable<CompareData>>(allData, requestModel.PageSize, requestModel.PageNo, counts, Count));
            }
            #endregion
        }
    }
}
