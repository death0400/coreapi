using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Data.Repository.CRUDApi.EntityKeyMap
{
    public static class EntityKeyDictionary
    {
        public static ConcurrentDictionary<Type, Type> TypeDictionary { get; set; } = new ConcurrentDictionary<Type, Type>();
        public static Type FindEntityKeyType<T>()
        {
            var type = typeof(T);
            return TypeDictionary.GetOrAdd(typeof(T),(_type) =>_type.GetProperties().Where(p => p.GetCustomAttributes(typeof(Key), false).Any()).First().PropertyType);
        }

    }
}
