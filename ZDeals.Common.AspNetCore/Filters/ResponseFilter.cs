using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Threading.Tasks;

using ZDeals.Common.AspNetCore.Mapping;

namespace ZDeals.Common.AspNetCore.Filters
{

    /// <summary>
    /// Convert the response in type Result<> to proper ActionResult
    /// </summary>
    public class ResponseFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult)
            {
                var result = context.Result as ObjectResult;
                var type = result.Value.GetType();

                if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var resultValue = result.Value as Result;
                    if (resultValue.HasError())
                    {
                        int status;
                        string message;
                        if (resultValue.HasError(ErrorType.Internal))
                        {
                            status = StatusCodes.Status500InternalServerError;
                            message = "Internal server error";
                        }
                        else if (resultValue.HasError(ErrorType.NotFound))
                        {
                            status = StatusCodes.Status404NotFound;
                            message = "The resource does not exist";
                        }
                        else if(resultValue.HasError(ErrorType.Validation))
                        {
                            status = StatusCodes.Status400BadRequest;
                            message = "Validation errors";
                        }
                        else
                        {
                            status = StatusCodes.Status400BadRequest;
                            message = "Bad request";
                        }

                        result.StatusCode = status;
                        result.Value = resultValue.Errors.ToErrorResponse(status, message);
                    }
                    else
                    {
                        var dataProperty = type.GetProperty("Data");
                        if (dataProperty != null)
                        {
                            result.Value = dataProperty.GetValue(resultValue);                            
                            if(result.Value == null)
                            {
                                result.StatusCode = StatusCodes.Status204NoContent;
                            }
                        }
                        else
                        {
                            result.StatusCode = StatusCodes.Status204NoContent;
                            result.Value = null;
                        }
                    }
                }
            }

            await next();
        }
    }
}
