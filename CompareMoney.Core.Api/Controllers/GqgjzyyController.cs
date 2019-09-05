using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CompareMoney.Core.Api.ControllersModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CompareMoney.Core.Api.Controllers
{
     /// <summary>
     /// 贵黔国际总医院
     /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GqgjzyyController : ControllerBase
    {


        /// <summary>
        /// 获得贵黔国际总医院Api.json 文件
        /// </summary>
        [HttpGet("GetJsonFile", Name = ("GetJsonFile"))]
        public IActionResult GetJsonFile() {

            //var config = new ConfigurationBuilder()
            // .SetBasePath(Directory.GetCurrentDirectory())
            // .AddJsonFile("postman_collection.json", optional: true, reloadOnChange: true)
            // .Build();

            //var list = config.Providers.FirstOrDefault();

            //var dyobj = (dynamic)list;
            //// var soure = dyobj.Source;
            // var obj = dyobj.Data;

            dynamic m = new ExpandoObject();
        
            string value = string.Empty;
            string jsonfile = "postman_collection.json";//File
            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                     value = o["item"].ToString();                 
                }
            }
            var obj = JsonConvert.DeserializeObject<object>(value);
            return Ok(new SucessModelData<object>(obj));

        }

 
    }




}
