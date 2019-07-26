using IServices.BaseServices;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.Services.Base
{
    public class BaseServicesInforPay<TEntity> : IBaseEntityInforPay<TEntity> where TEntity : class, new()
    {

        public IBaseEntityInforPay<TEntity> BaseDal;



        /// <summary>
        /// 新增一个数据库对象模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public  async Task<TEntity> Add(TEntity model)
        {
            return await BaseDal.Add(model);

            // throw new NotImplementedException();
        }

        /// <summary>
        /// 返回是影响的行数
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>

        public  async Task<int> Count(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await BaseDal.Count(whereLambda);

         
        }

        /// <summary>
        /// 删除对象 返回受影响的行数
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns></returns>
        public async Task<int> DelBy(Expression<Func<TEntity, bool>> delWhere)
        {


            return await BaseDal.DelBy(delWhere);
       
        }

        /// <summary>
        /// 查询Model
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> whereLambda)
        {

            return await BaseDal.GetModelAsync(whereLambda);
           
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderByLambda, bool isAsc = true)
        {

            return await BaseDal.GetPagedList(pageIndex,pageSize,whereLambda,orderByLambda,isAsc);
          
        }

     

        /// <summary>
        /// 查询list对象
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereLambda)
        {

            return await BaseDal.Query(whereLambda);

           
        }
    }
}
