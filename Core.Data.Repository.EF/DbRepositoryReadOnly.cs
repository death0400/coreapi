using Microsoft.EntityFrameworkCore;

namespace Core.Data.Repository.EF
{
    public class DbRepositoryReadOnly<T, TKey> : DbBaseRepository<T,TKey>, IRepositoryReadOnly<T> where T : class
    {
        public DbRepositoryReadOnly(DbContext context) : base(context)
        {
        }
    }
}