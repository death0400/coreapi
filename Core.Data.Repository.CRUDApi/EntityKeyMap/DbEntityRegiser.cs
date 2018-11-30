using Core.Data.Repository.CRUDApi.FeatureProvider;
using Core.Data.Repository;
using Core.Data.Repository.EF;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Data.Repository.CRUDApi.EntityKeyMap
{
    public class DbEntityRegiser<TContext> : IEntityRegister<TContext> where TContext : DbContext
    {
        private TContext context;
        private IServiceCollection services;
        public IMvcBuilder MvcBuilder { get; set; }
        public DbEntityRegiser(TContext context,IMvcBuilder mvcBuilder)
        {
            this.context = context;
            this.services = mvcBuilder.Services;
            this.MvcBuilder = mvcBuilder;
            var types = typeof(TContext).GetProperties()
            .Where(prop => prop.PropertyType.IsGenericType)
            .Where(prop => prop.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(prop => prop.PropertyType.GenericTypeArguments.First())
            .Distinct();
            foreach (var type in types)
            {
                EntityKeyDictionary.TypeDictionary.TryAdd(type, context.Model.FindEntityType(type).FindPrimaryKey().Properties.First().PropertyInfo.PropertyType);
            }
        }
        public DbEntityRegiser<TContext> RegisterEntityType<T>() where T : class
        {
            services.AddScoped<RepositoryFeatureProvider<T>>();
            services.AddScoped<IRepository<T>>(provider =>
            {
                return (IRepository<T>)Activator.CreateInstance(typeof(DbRepository<,>).MakeGenericType(new Type[] { typeof(T), EntityKeyDictionary.TypeDictionary[typeof(T)] }), provider.GetRequiredService<TContext>());
            });
            MvcBuilder
                .ConfigureApplicationPartManager(p =>
                {
                    p.FeatureProviders.Add(new RepositoryFeatureProvider<T>());
                });
            return this;
        }
        public DbEntityRegiser<TContext> RegisterAllEntityTypes()
        {
            foreach (var keyValuePair in EntityKeyDictionary.TypeDictionary)
            {
                if (typeof(TContext).GetTypeInfo().Assembly != keyValuePair.Key.GetTypeInfo().Assembly)
                    continue;
                var featureProviderType = typeof(RepositoryFeatureProvider<>).MakeGenericType(keyValuePair.Key);
                services.AddScoped(featureProviderType);
                services.AddScoped(typeof(IRepository<>).MakeGenericType(keyValuePair.Key), provider =>
                    Activator.CreateInstance(typeof(DbRepository<,>).MakeGenericType(keyValuePair.Key, keyValuePair.Value), provider.GetRequiredService<TContext>()));
                MvcBuilder
                    .ConfigureApplicationPartManager(p =>
                    {
                        p.FeatureProviders.Add((IApplicationFeatureProvider)services.BuildServiceProvider().GetRequiredService(featureProviderType));
                    });
            }
            return this;
        }
        IEntityRegister<TContext> IEntityRegister<TContext>.RegisterEntityType<T>()
        {
            return this.RegisterEntityType<T>();
        }
    }
}
