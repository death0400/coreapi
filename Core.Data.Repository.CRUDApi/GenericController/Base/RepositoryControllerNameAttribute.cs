using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Repository.CRUDApi.GenericController.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RepositoryControllerNameAttribute : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.GetGenericTypeDefinition() == typeof(RepositoryController<,>))
            {
                var entityType = controller.ControllerType.GenericTypeArguments[0];
                controller.ControllerName =entityType.FullName;
            }
        }
    }
}
