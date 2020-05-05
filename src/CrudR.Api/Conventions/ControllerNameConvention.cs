using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CrudR.Api.Conventions
{
    public class ControllerNameConvention : IControllerModelConvention
    {
        private readonly string _controllerName;

        public ControllerNameConvention(string controllerName) =>
            _controllerName = controllerName;

        public void Apply(ControllerModel controller)
        {
            _ = controller ?? throw new ArgumentNullException(nameof(controller));

            controller.ControllerName = _controllerName;
        }
    }
}
