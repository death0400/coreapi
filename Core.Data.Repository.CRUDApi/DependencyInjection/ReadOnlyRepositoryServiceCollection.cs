using Core.Data.Repository.CRUDApi.FeatureProvider;
using Core.Data.Repository.CRUDApi.GenericController;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.Repository.CRUDApi.DependencyInjection
{
    public static class ReadOnlyRepositoryServiceCollection
    {
        public static IMvcBuilder AddReadOnlyRepositoryWebApi<T,TKey>(this IMvcBuilder mvcBuilder,Action<ReadOnlyRepositoryOptions<T>> action) where T:class
        {
            action(new ReadOnlyRepositoryOptions<T>(mvcBuilder.Services));
            mvcBuilder.Services.AddScoped<ReadRepositoryController<T, TKey>>();
            mvcBuilder.ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new ReadRepositoryFeatureProvider<T,TKey>()));
            return mvcBuilder;
        }

    }
    public class ReadOnlyRepositoryOptions<T> where T:class
    {
        IServiceCollection services;
        public ReadOnlyRepositoryOptions(IServiceCollection services)
        {
            this.services = services;
        }
        public void RegisterIReadRepository<TRepository>() 
            where TRepository :class, IReadRepository<T>
        {
            services.AddScoped<IReadRepository<T>, TRepository>();
        }
    }
}
