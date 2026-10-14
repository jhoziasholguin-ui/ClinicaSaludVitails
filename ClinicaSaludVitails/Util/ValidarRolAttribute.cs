using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClinicaSaludVitails.Util
{
    public class ValidarRolAttribute : ActionFilterAttribute
    {
        private readonly string[] _rolesPermitidos;

        public ValidarRolAttribute(params string[] roles)
        {
            _rolesPermitidos = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            string? userRole = session.GetString("UserRole");

            // Si no hay sesión o el rol no está en la lista de permitidos, rebota al Login
            if (string.IsNullOrEmpty(userRole) || !_rolesPermitidos.Contains(userRole))
            {
                filterContext.Result = new RedirectToActionResult("Login", "Account", null);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}