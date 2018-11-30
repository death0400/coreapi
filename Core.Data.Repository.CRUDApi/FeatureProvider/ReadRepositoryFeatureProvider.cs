using Core.Data.Repository.CRUDApi.DependencyInjection;
using Core.Data.Repository.CRUDApi.EntityKeyMap;
using Core.Data.Repository.CRUDApi.GenericController;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Data.Repository.CRUDApi.FeatureProvider
{
    public class ReadRepositoryFeatureProvider<T,TKey> : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            // Get the list of entities that we want to support for the generic controller
            var entityType = typeof(T);
            var entityKeyType = typeof(TKey);
            var typeName = entityType.Name + "Controller";
                // Check to see if there is a "real" controller for this class
                if (!feature.Controllers.Any(t => t.Name == typeName))
                {
                    // Create a generic controller for this type
                    var controllerType = typeof(ReadRepositoryController<,>).MakeGenericType(new Type[] { entityType, entityKeyType }).GetTypeInfo();
                    feature.Controllers.Add(controllerType);
                }
            
        }
    }
}
