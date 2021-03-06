﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareMoney.Core.Api.ControllersModels
{
    public class GetALLDataPageModel
    {

        /// <summary>
        /// 页数
        /// </summary>
        public int PageNo { get; set; } = 1;

        /// <summary>
        /// 页码大小
        /// </summary>

        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool Sorter { get; set; } = true;

        /// <summary>
        /// 日期数组
        /// </summary>

        public string[] BillDate { get; set; } = null;


        /// <summary> 
        /// 明细查询的 类型 默认查询全部
        /// </summary>
        public int IsTrue { get; set; } = 3;
    }
}
