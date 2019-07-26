using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareMoney.Core.Api.ControllersModels
{
    public class SucessModel
    {

        public int Code { get; set; } = 200;

        public string Msg { get; set; } = "操作成功";
    }

    public class SucessModelData<T> : SucessModel
    {
        public T Data { get; set; }

        public SucessModelData(T data)
        {
            this.Data = data;
        }
    }

    public class SucessModelCount : SucessModel
    {
        public int Count { get; set; } = 0;

        public SucessModelCount(int count)
        {
            this.Count = count;

            this.Msg = "操作成功,共增加" + count + "条数据";
        }
    }
    public class SuccessDataPages<T> : SucessModel
    {
        public int PageSize { get; set; }

        public int PageNo { get; set; }

        public int TotalPage { get; set; }

        public int TotalCount { get; set; }

        public T Data { get; set; }

        public SuccessDataPages(T data, int pageSize, int pageNo, int totalPage, int totalCount)
        {
            this.Data = data;
            this.PageSize = pageSize;
            this.PageNo = pageNo;
            this.TotalPage = totalPage;
            this.TotalCount = totalCount;
        }




    }
      public class JsonFailCatch : SucessModel
    {

        public JsonFailCatch(string msg)
        {
            this.Code = 500;
            this.Msg = msg;
        }
    }




}
