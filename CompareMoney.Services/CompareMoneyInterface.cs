using CompareMoney.Applicaion.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.Services
{

    /// <summary>
    /// 业务逻辑类的接口
    /// </summary>
    public interface ICompareMoneyInterface
    {
        Task<List<poolModel>> DetailedList(string[] BillDate); //第一层比对

        Task<List<CompareData>> DetailedListAll(string[] BillDate); //第二次比对

        Task<List<CompareData>> DetailedListError(string[] BillDate);//找出错误的数据
    }
}
