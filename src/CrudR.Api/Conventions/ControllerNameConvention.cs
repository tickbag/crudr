using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CrudR.Api.Conventions
{
    /// <summary>
    /// Convention to set all controller names to be the provided name in the constructor.
    /// This should only be used in a project with a single controller.
    /// </summary>
    public class ControllerNameConvention : IControllerModelConvention
    {
        private readonly string _controllerName;

        /// <summary>
        /// The Convention Constructor
        /// </summary>
        /// <param name="controllerName">The new name for the controller</param>
        public ControllerNameConvention(string controllerName) =>
            _controllerName = controllerName;

        /// <inheritdoc/>
        public void Apply(ControllerModel controller)
        {
            _ = controller ?? throw new ArgumentNullException(nameof(controller));

            if (_controllerName != null)
                controller.ControllerName = _controllerName;
        }
    }
}
