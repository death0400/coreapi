using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Concurrent;

namespace Core.Data
{
    public class KeyWrapper<TContext> where TContext : DbContext
    {
        TContext dbContext;

        public KeyWrapper(TContext dbContext)
        {
            this.dbContext = dbContext;
            AddKeyType();
        }
        public void AddKeyType()
        {
            var types = typeof(TContext).GetProperties()
                .Where(prop => prop.PropertyType.IsGenericType)
                .Where(prop => prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(prop => prop.PropertyType.GenericTypeArguments.First())
                .Distinct();

            foreach (var type in types)
            {
                KeyWrapper.KeyTypes.GetOrAdd(type, (_type) =>
                dbContext.Model.FindEntityType(_type).FindPrimaryKey().Properties.First().PropertyInfo.PropertyType);
            }

        }
    }
    public static class KeyWrapper
    {
        public static ConcurrentDictionary<Type, Type> KeyTypes { get; set; } = new ConcurrentDictionary<Type, Type>();
    }
}
