using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyEmployees.Presentation.ActionFilters;

public class ValidationFilterAttribute : IActionFilter
{
    public ValidationFilterAttribute() { }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Extract action name.
        var action = context.RouteData.Values["action"];
        // Extract controller name.
        var controller = context.RouteData.Values["controller"];

        // Extract dto parameter that we send to the POST and PUT actions.
        var param = context.ActionArguments
            .SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;

        if (param is null)
        {
            context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
            return;
        }

        // Check if model is invalid.
        if (!context.ModelState.IsValid)
        {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
