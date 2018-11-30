using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Repository.CRUDApi.GenericController.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ReadRepositoryControllerNameAttribute : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.GetGenericTypeDefinition() == typeof(ReadRepositoryController<,>))
            {
                var type = controller.ControllerType.GenericTypeArguments[0];
                controller.ControllerName = type.FullName;
            }
        }
    }
}
