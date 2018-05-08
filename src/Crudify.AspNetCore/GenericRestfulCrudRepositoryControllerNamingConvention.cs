using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crudify.AspNetCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class GenericRestfulCrudRepositoryControllerNamingConvention : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var firstGenericTypeArgumentType = controller.ControllerType.GenericTypeArguments[0];
            controller.ControllerName = firstGenericTypeArgumentType.Name;
        }
    }
}
