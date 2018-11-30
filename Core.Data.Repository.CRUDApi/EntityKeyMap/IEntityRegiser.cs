using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.Repository.CRUDApi.EntityKeyMap
{
    public interface IEntityRegister<TContext> 
    {
        IEntityRegister<TContext> RegisterEntityType<T>() where T : class;
    }
}
