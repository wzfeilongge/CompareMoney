﻿using CompareMoney.IRepository.Base;
using CompareMoney.Repository.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CompareMoney.Repository.Base
{
    /// <summary>
    /// pay的数据库对象集合
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepositoryPay<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        public EfDbcontextRepositoryPay Context { get; set; }

        internal DbSet<TEntity> Dbset { get; set; }

        private readonly ILogger<BaseRepositoryPay<TEntity>> _myLogger;

        public BaseRepositoryPay(ILogger<BaseRepositoryPay<TEntity>> myLogger)
        {
            _myLogger = myLogger;
             Context = new EfDbcontextRepositoryPay();
            Dbset = Context.Set<TEntity>();
            _myLogger.LogInformation($"Pay Model{Context.Model.GetEntityTypes()}");
        }
      

  
        #region 1.0 新增实体, 返回受影响的行数
        public async Task<int> AddModel(TEntity model)
        {
            _myLogger.LogInformation($"Pay Model{Context.Model} 正在执行新增Model 返回行数");
            await Dbset.AddAsync(model);
            return await Context.SaveChangesAsync();

        }
        #endregion

        #region 1.0 新增实体，返回实体
        /// <summary>
        /// 1.0 新增实体，返回受影响的行数
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响的行数</returns>
        public async Task<TEntity> Add(TEntity model)
        {
            _myLogger.LogInformation($"Pay Model{Context.Model} 正在执行增加Model 返回Model");
            await Dbset.AddAsync(model);
            Context.SaveChanges();
            return model;

        }
        #endregion

        #region 1.0根据条件查询
        /// <summary>
        /// 1.0根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<int> Count(Expression<Func<TEntity, bool>> whereLambda)
        {
            // .Count(whereLambda);
            _myLogger.LogInformation($"Pay Model{Context.Model} 正在执行查询返回受影响的行数");
            return await Dbset.CountAsync(whereLambda);


        }
        #endregion

        #region 1.0 根据条件删除
        /// <summary>
        /// 1.0 根据条件删除
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> DelBy(Expression<Func<TEntity, bool>> delWhere)
        {
            _myLogger.LogInformation($"Pay Model{Context.Model} 正在执行条件删除");
            var listDeleting = await Dbset.Where(delWhere).ToListAsync();
            listDeleting.ForEach(u =>
            {
                Dbset.Attach(u);  //先附加到EF 容器
                Dbset.Remove(u); //标识为删除状态
            });

            return Context.SaveChanges();

        }
        #endregion

        #region 1.0 根据条件查询单个model
        /// <summary>
        /// 4.0 根据条件查询单个model
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> whereLambda)
        {
            _myLogger.LogInformation($"Pay Model{Context.Model} 正在执行查询单个Model");
            return await Dbset.Where(whereLambda).AsNoTracking().FirstOrDefaultAsync();
        }
        #endregion

        #region 1.0 分页查询
        /// <summary>
        /// 分页查询 + List<T> GetPagedList
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件 lambda表达式</param>
        /// <param name="orderBy">排序 lambda表达式</param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderByLambda, bool isAsc = true)
        {
            _myLogger.LogInformation($"Pay Model{Context.Model} 正在执行分页查询");
            if (isAsc)
            {
                return await Dbset.Where(whereLambda).OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }
            else
            {
                return await Dbset.Where(whereLambda).OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }





            //throw new NotImplementedException();

        }
        #endregion

        #region 1.0 根据条件查询
        /// <summary>
        /// 1.0 根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereLambda)
        {
            _myLogger.LogInformation($"Pay Model{Context.Model} 正在执行查询");
            return await Dbset.Where(whereLambda).AsNoTracking().ToListAsync();


        }
        #endregion
    }
}
