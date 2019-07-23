using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareMoney.Core.Api.ControllersModels
{
    public class SucessModel
    {

        public int code { get; set; } = 200;

        public string msg { get; set; } = "操作成功";



    }

    public class SucessModelData<T> : SucessModel
    {

      //  public int code { get; set; } = 200;

       // public string msg { get; set; } = "操作成功";


        public T data { get; set; }



        public SucessModelData(T data)
        {
            this.data = data;
        }


    }

    public class SucessModelCount :SucessModel
    {

      //  public  int code { get; set; } = 200;

        public   int count { get; set; } = 0;

     //   public  string msg { get; set; } 


        public SucessModelCount(int count)
        {
            this.count = count;

            this.msg= "操作成功,共增加" + count + "条数据";


        }



    }


    public class SuccessDataPages<T> : SucessModel
    {

       // public int code { get; set; } = 200;

      //  public string msg { get; set; } = "操作成功";

        public int pageSize { get; set; }

        public int pageNo { get; set; }

        public int totalPage { get; set; }

        public int totalCount { get; set; }

        public T data { get; set; }

        public SuccessDataPages(T data, int pageSize, int pageNo, int totalPage, int totalCount)
        {
            this.data = data;
            this.pageSize = pageSize;
            this.pageNo = pageNo;
            this.totalPage = totalPage;
            this.totalCount = totalCount;
     }




    }


    public class JsonFailCatch : SucessModel
    {


        //public int code { get; set; } = 500;

       //  public string msg { get; set; }


        public JsonFailCatch(string msg)
        {
            this.code = 500;
            this.msg = msg;
        }



    }




}
