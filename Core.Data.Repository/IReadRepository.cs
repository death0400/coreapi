using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Core.Data.Paging;
using System.Collections.Generic;

namespace Core.Data.Repository
{
    public interface IReadRepository<T> where T : class
    {
        T Get(params object[] keyValues);
        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null);
        IEnumerable<T> GetList(int amount);

    }
}