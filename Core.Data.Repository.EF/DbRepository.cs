using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repository.EF
{



    public class DbRepository<T,TKey> : DbBaseRepository<T,TKey>, IRepository<T> where T : class
    {
        public DbRepository(DbContext context) : base(context)
        {
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Add(params T[] entities)
        {
            _dbSet.AddRange(entities);
        }


        public void Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }


        void IRepository<T>.Delete(object key)
        {

            Delete(_dbSet.Find((TKey)key));
        }
        public void Delete(T entity)
        {
            if (entity != null) _dbSet.Remove(entity);
        }


        public void Delete(TKey key)
        {
            Delete(_dbSet.Find(key));
        }

        public void Delete(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }


        [Obsolete("Method is replaced by GetList")]
        public IEnumerable<T> Get()
        {
            return _dbSet.AsEnumerable();
        }

        [Obsolete("Method is replaced by GetList")]
        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsEnumerable();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Update(params T[] entities)
        {
            _dbSet.UpdateRange(entities);
        }


        public void Update(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}