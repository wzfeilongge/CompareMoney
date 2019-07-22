
using CompareMoney.IRepository.Base;
using IServices.BaseServices;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.Services.Base
{
    public class BaseServicesInforHis<TEntity> : IBaseEntityInforHis<TEntity> where TEntity : class, new()
    {



        public IBaseRepository<TEntity> BaseDal;

        public async Task<TEntity> Add(TEntity model)
        {
            return await BaseDal.Add(model);

          
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await BaseDal.Count(whereLambda);

           
        }

        public async Task<int> DelBy(Expression<Func<TEntity, bool>> delWhere)
        {


            return await BaseDal.DelBy(delWhere);

        }

        public async Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> whereLambda)
        {

            return await BaseDal.GetModelAsync(whereLambda);

        }

        public async Task<List<TEntity>> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderByLambda, bool isAsc = true)
        {

            return await BaseDal.GetPagedList(pageIndex, pageSize, whereLambda, orderByLambda, isAsc);
           
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereLambda)
        {

            return await BaseDal.Query(whereLambda);

           
        }
    }
}
