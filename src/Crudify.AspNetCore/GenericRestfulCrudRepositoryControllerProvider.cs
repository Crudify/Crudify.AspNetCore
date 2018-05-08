using Crudify.Core;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Crudify.AspNetCore
{
    public class GenericRestfulCrudRepositoryControllerProvider :
        IApplicationFeatureProvider<ControllerFeature>
    {
        public GenericRestfulCrudRepositoryControllerProvider(List<Type> iEntityCrudRepositoryTypes)
        {
            foreach (var iEntityCrudRepositoryType in iEntityCrudRepositoryTypes)
            {
                if (iEntityCrudRepositoryType.Name != typeof(ICrudRepository<,>).Name)
                {
                    throw new ArgumentException($"Only allowed to pass types that implement {typeof(ICrudRepository<,>).Name}");
                }
            }

            IEntityCrudRepositoryTypes = iEntityCrudRepositoryTypes;
        }

        public List<Type> IEntityCrudRepositoryTypes { get; }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            // This is designed to run after the default ControllerTypeProvider, 
            // so the list of 'real' controllers has already been populated.
            foreach (var iEntityCrudRepositoryType in IEntityCrudRepositoryTypes)
            {
                var tEntity = iEntityCrudRepositoryType.GenericTypeArguments[0];
                var tEntityId = iEntityCrudRepositoryType.GenericTypeArguments[1];

                var controllerType = typeof(ICrudRepository<,>)
                    .MakeGenericType(iEntityCrudRepositoryType.GenericTypeArguments).GetTypeInfo();

                feature.Controllers.Add(controllerType);
            }
        }
    }
}
