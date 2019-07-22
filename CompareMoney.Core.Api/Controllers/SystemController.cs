using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompareMoney.Applicaion.ViewModel;
using CompareMoney.Core.Api.ControllersModels;
using CompareMoney.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;

namespace CompareMoney.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly CompareMoneyInterface _compareMoneyInterface;

        public SystemController(CompareMoneyInterface compareMoneyInterface)
        {

            _compareMoneyInterface = compareMoneyInterface;

        }

        /// <summary>
        /// 根据时间获取pay和His的非明细的数据
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpGet("{GetBillPage}", Name = "GetBillPage")]
        public async Task<IActionResult> GetBillPage([FromBody] GetBillPageModel requestModel)
        {

            using (MiniProfiler.Current.Step("用户正在请求第一个界面的查询"))
            {

                if (requestModel.BillDate.Length > 10)
                {
                

                    return Ok(new JsonFailCatch("查询的数据不能超过10天"));

                }
                else if (requestModel.BillDate.Length == 0)
                {
                    return Ok(new object());

                }



                var result = await _compareMoneyInterface.DetailedList(requestModel.BillDate);
                var Count = result.Count;

                var CountPage = Count / requestModel.pageSize;
                if (Count % requestModel.pageSize >= 1)
                {
                    CountPage++;
                }
                if (CountPage < 1)
                {
                    CountPage = 1;
                }


                return Ok(new SuccessDataPages<List<poolModel>>(result, requestModel.pageSize, requestModel.pageNo, CountPage, Count));



            }
        }




        /// <summary>
        /// 根据时间获取明细的数据
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>

        [HttpGet("{GetALLDataPage}", Name = "GetALLDataPage")]
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


            using (MiniProfiler.Current.Step("用户正在请求第二个界面的查询"))
            {
                if (requestModel.isTrue == 0)
                {
                    var errorList = await _compareMoneyInterface.DetailedListError(requestModel.BillDate);
                    var Data = errorList.Where(obj => obj.isTrue == 0).OrderBy(obj => obj.transactionTime).Skip((requestModel.pageNo - 1) * requestModel.pageSize).Take(requestModel.pageSize).ToList();
                    var falseCount = Data.Count();


                    var Counts = Data.Count;
                    if (Counts < 1)
                    {
                        Counts = 1;
                    }
                    var CountPages = Counts / requestModel.pageSize;
                    if (Counts % requestModel.pageSize >= 1)
                    {
                        CountPages++;
                    }
                    if (CountPages < 1)
                    {
                        CountPages = 1;
                    }
                    return Ok(new SuccessDataPages<IEnumerable<CompareData>>(Data, requestModel.pageSize, requestModel.pageNo, CountPages, falseCount));
                }
                var result = await _compareMoneyInterface.DetailedListAll(requestModel.BillDate);
                var Count = result.Count;
                if (Count < 1)
                {
                    Count = 1;
                }
                var CountPage = Count / requestModel.pageSize;
                if (Count % requestModel.pageSize >= 1)
                {
                    CountPage++;
                }
                if (CountPage < 1)
                {
                    CountPage = 1;
                }
                IEnumerable<CompareData> trueData = null;
                //if (requestModel.isTrue == 0)
                //{
                //    var errorList = await _compareMoneyInterface.DetailedListError(requestModel.BillDate);
                //    trueData = errorList.Where(obj => obj.isTrue == 0).OrderBy(obj => obj.transactionTime).Skip((requestModel.pageNo - 1) * requestModel.pageSize).Take(requestModel.pageSize);
                //    var falseCount = errorList.Where(obj => obj.isTrue == 0).Count();

                //    return Ok(new SuccessDataPages<IEnumerable<CompareData>>(trueData, requestModel.pageSize, requestModel.pageNo, CountPage, falseCount));
                // }
                if (requestModel.isTrue == 1)
                {
                    trueData = result.Where(obj => obj.isTrue == 1).OrderBy(obj => obj.transactionTime).Skip((requestModel.pageNo - 1) * requestModel.pageSize).Take(requestModel.pageSize).ToArray();

                    var trueCount = result.Where(obj => obj.isTrue == 1).Count();

                    return Ok(new SuccessDataPages<IEnumerable<CompareData>>(trueData, requestModel.pageSize, requestModel.pageNo, CountPage, trueCount));
                }
                else
                {
                    return Ok(new SuccessDataPages<IEnumerable<CompareData>>(result, requestModel.pageSize, requestModel.pageNo, CountPage, Count));
                }
            }

        }












    }
}
