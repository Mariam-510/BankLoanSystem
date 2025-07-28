using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BankLoanSystem.API.ResponseModels;

namespace BankLoanSystem.API.Filters
{
    public class ApiResponseFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var response = ApiResponse<object>.SuccessResponse(
                    objectResult.Value,
                    "Operation successful",
                    objectResult.StatusCode ?? 200);

                context.Result = new ObjectResult(response)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
            
            else if (context.Result is EmptyResult)
            {
                var response = ApiResponse<object>.SuccessResponse(
                    null,
                    "Operation successful",
                    200);

                context.Result = new ObjectResult(response);
            }
            
            else if (context.Result is BadRequestObjectResult badRequest)
            {
                var errors = new List<string>();

                if (badRequest.Value is SerializableError serializableError)
                {
                    errors = serializableError.SelectMany(e =>
                        ((string[])e.Value).Select(v => $"{e.Key}: {v}"))
                        .ToList();
                }
                else if (badRequest.Value is string errorMessage)
                {
                    errors.Add(errorMessage);
                }

                var response = ApiResponse<object>.ErrorResponse(
                    "Validation failed",
                    400,
                    errors);

                context.Result = new ObjectResult(response)
                {
                    StatusCode = 400
                };
            }
        }
    }
}
