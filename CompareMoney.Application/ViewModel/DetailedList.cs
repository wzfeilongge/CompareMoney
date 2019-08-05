using System;
using System.Collections.Generic;
using System.Text;

namespace CompareMoney.Applicaion.ViewModel
{
   public class DetailedList
    {
        //数据模型 尚未进入数据库
        public string BillDate { get; set; } //哪天

        public double Money { get; set; } //金额汇总

        public int Count { get; set; } //数据总量

        public string DataName { get; set; } //是哪里的数据
    }
}
