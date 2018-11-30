using Core.Data.Repository.CRUDApi.DependencyInjection;
using Core.Data.Repository.CRUDApi.EntityKeyMap;
using Core.Data.Repository.CRUDApi.FeatureProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core.Data.Repository.CRUDApi.DependencyInjection
{
    public static class DbEntityRepositoryServiceCollection
    { 
        public static IMvcBuilder AddDbRepositoryWebApi<TContext>(this IMvcBuilder mvcBuilder, Action<DbEntityRegiser<TContext>> action) 
            where TContext : DbContext, new()
        {
            var dbRegister = new DbEntityRegiser<TContext>(mvcBuilder.Services.BuildServiceProvider().GetRequiredService<TContext>(),  mvcBuilder);
            action(dbRegister);
            return mvcBuilder;
        }
    }
}
