using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Service.DbHelper
{
    public interface IRepository : IDisposable
    {
        T Single<T>(object key) where T : class, IEntity, new();
        T Single<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity, new();

        int Count<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity;

        bool Any<T>(object key) where T : class, IEntity;
        bool Any<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity;

        List<T> All<T>(IDbTransaction trans = null) where T : class, IEntity, new();
        //List<T> All<T>(IPager pager, string orderBy = null) where T : class, IEntity, new();

        List<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity, new();
        //List<T> Query<T>(IPager pager, Expression<Func<T, bool>> predicate, string orderBy = null) where T : class, IEntity, new();
        //List<T> Query<T>(Criteria criteria) where T : class, IEntity, new();

        int Insert<T>(T instance) where T : class, IEntity;
        int Insert<T>(T[] instance) where T : class, IEntity;

        //int Update<T>(T instance) where T : class, IEntity;
        //int Update<T>(T instance, Expression<Func<T, bool>> predicate) where T : class, IEntity;

        int Save<T>(T instance) where T : class, IEntity;
        int Save<T>(T[] instances) where T : class, IEntity;
        //int Save<T>(T instance, out object key) where T : class, IEntity;

        //int Save<T>(T instance, Expression<Func<T, bool>> predicate) where T : class, IEntity;

        int Delete<T>(object key) where T : class, IEntity;
        int Delete<T>(T instance) where T : class, IEntity;
        int Delete<T>(T[] instances) where T : class, IEntity;
        int Delete<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity;

        int ExecuteSqlCommand(string sql, object[] param);
    }
}