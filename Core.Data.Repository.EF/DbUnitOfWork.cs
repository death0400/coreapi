using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repository.EF
{
    public class DbUnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>
        where TContext : DbContext, IDisposable
    {
        private Dictionary<Type, object> _repositories;
        private KeyWrapper<TContext> _keyWrapper;
        public TContext Context { get; }

        public DbUnitOfWork(TContext context, KeyWrapper<TContext> keyWrapper)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            _keyWrapper = keyWrapper;
        }

        //public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        //{
        //    if (_repositories == null)
        //        _repositories = new Dictionary<Type, object>();
        //    var type = typeof(TEntity);
        //    if (!_repositories.ContainsKey(type)) _repositories[type] = new DbRepository<TEntity>(Context);
        //    return (IRepository<TEntity>)_repositories[type];
        //}

        public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type)) _repositories[type] = new DbRepositoryAsync<TEntity>(Context);
            return (IRepositoryAsync<TEntity>)_repositories[type];
        }

        //public IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : class
        //{
        //    if (_repositories == null) _repositories = new Dictionary<Type, object>();

        //    var type = typeof(TEntity);
        //    if (!_repositories.ContainsKey(type)) _repositories[type] = new DbRepositoryReadOnly<TEntity>(Context);
        //    return (IRepositoryReadOnly<TEntity>)_repositories[type];
        //}
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }
        public void Dispose()
        {
            Context?.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}