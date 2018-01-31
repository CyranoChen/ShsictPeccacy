using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Service.DbHelper
{
    public class Repository : IRepository
    {
        private OracleDbContext _db;

        public Repository(OracleDbContext ctx = null)
        {
            _db = ctx ?? new OracleDbContext();
        }

        public T Single<T>(object key) where T : class, IEntity, new()
        {
            return _db.Set<T>().Find(key);
        }

        public T Single<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity, new()
        {
            return _db.Set<T>().Single(predicate);
        }

        public int Count<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return _db.Set<T>().Count(predicate);
        }

        public bool Any<T>(object key) where T : class, IEntity
        {
            return _db.Set<T>().Find(key) != null;
        }

        public bool Any<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            return Count(predicate) > 0;
        }

        public List<T> All<T>(IDbTransaction trans = null) where T : class, IEntity, new()
        {
            return _db.Set<T>().ToList();
        }

        public List<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity, new()
        {
            return _db.Set<T>().Where(predicate).ToList();
        }

        public int Insert<T>(T instance) where T : class, IEntity
        {
            _db.Set<T>().Add(instance);

            return _db.SaveChanges();
        }

        //public int Insert<T>(T instance, out object key) where T : class, IEntity
        //{
        //    _db.Set<T>().Add(instance);

        //    var ret = _db.SaveChanges();

        //    key = instance.ID;

        //    return ret;
        //}

        public int Save<T>(T instance) where T : class, IEntity
        {
            _db.Set<T>().AddOrUpdate(instance);

            return _db.SaveChanges();
        }

        public int Save<T>(T[] instances) where T : class, IEntity
        {
            _db.Set<T>().AddOrUpdate(instances);

            return _db.SaveChanges();
        }

        //public int Save<T>(T instance, out object key) where T : class, IEntity
        //{
        //    _db.Set<T>().AddOrUpdate(instance);

        //    var ret = _db.SaveChanges();

        //    key = instance.ID;

        //    return ret;
        //}

        public int Delete<T>(object key) where T : class, IEntity
        {
            var entity = _db.Set<T>().Find(key);

            return entity != null ? Delete(entity) : 0;
        }

        public int Delete<T>(T instance) where T : class, IEntity
        {
            _db.Set<T>().Remove(instance);

            return _db.SaveChanges();
        }

        public int Delete<T>(Expression<Func<T, bool>> predicate) where T : class, IEntity
        {
            var instances = _db.Set<T>().Where(predicate);

            if (instances.Any())
            {
                _db.Set<T>().RemoveRange(instances);

                return _db.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}