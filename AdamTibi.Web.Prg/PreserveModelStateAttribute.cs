using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdamTibi.Web.Prg
{
    // Ref https://andrewlock.net/post-redirect-get-using-tempdata-in-asp-net-core/
    /// <summary>
    /// Works in conjunction with <c>RestoreModelStateAttribute</c> to preserve the ModelState through TempData (You need to enable TempData <see cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.2#tempdata"/>
    /// </summary>
    public class PreserveModelStateAttribute : ActionFilterAttribute
    {
        private readonly bool _alwaysPreserve;
        private readonly string _key;

        /// <summary>
        /// Causes the ModelState on the action method to be stored in the TempData to be used by the <c>RestoreModelStateAttribute</c>
        /// </summary>
        /// <param name="name">A unique name that should be used on the related <c>PreserveModelStateAttribute</c> and <c>RestoreModelStateAttribute</c></param>
        /// <param name="alwaysPreserve">This will always preserve the ModelState including when ModelState.IsValid is true</param>
        public PreserveModelStateAttribute(string name, bool alwaysPreserve = false)
        {
            _alwaysPreserve = alwaysPreserve;
            _key = "ModelState_" + name;
        }
        
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // If the ModelState is valid then probably we won't be using the PRG method, so don't store the state
            // unless the user really wants to
            if (!_alwaysPreserve && filterContext.ModelState.IsValid)
            {
                return;
            } 
            
            //Export if we are redirecting
            if (!(filterContext.Result is RedirectResult) 
                && !(filterContext.Result is RedirectToRouteResult) 
                && !(filterContext.Result is RedirectToActionResult))
            {
                return;
            }

            if (!(filterContext.Controller is Controller controller) || filterContext.ModelState == null)
            {
                return;
            }

            string modelState = ModelStateHelpers.SerialiseModelState(filterContext.ModelState);
            controller.TempData[_key] = modelState;
            base.OnActionExecuted(filterContext);
        }
    }
}
