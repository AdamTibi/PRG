using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdamTibi.Web.Prg
{
    // Ref https://andrewlock.net/post-redirect-get-using-tempdata-in-asp-net-core/
    /// <summary>
    /// Works in conjunction with <c>PreserveModelStateAttribute</c> to restore the ModelState through TempData (You need to enable TempData <see cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.2#tempdata"/>
    /// </summary>
    public class RestoreModelStateAttribute : ActionFilterAttribute
    {
        private readonly string _key;

        /// <summary>
        /// Causes the ModelState stored earlier by the <c>PreserveModelStateAttribute</c> to be retrieved from the TempData
        /// </summary>
        /// <param name="name">A unique name that should be used on the related <c>PreserveModelStateAttribute</c> and <c>RestoreModelStateAttribute</c></param>
        public RestoreModelStateAttribute(string name)
        {
            _key = "ModelState_" + name;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            Controller controller = filterContext.Controller as Controller;

            if (!(controller?.TempData[_key] is string serialisedModelState))
            {
                return;
            }

            //Only Import if we are viewing
            if (!(filterContext.Result is ViewResult))
            {
                return;
            }

            ModelStateDictionary modelState = ModelStateHelpers.DeserialiseModelState(serialisedModelState);
            filterContext.ModelState.Merge(modelState);
        }
    }
}
