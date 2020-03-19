using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

using ZDeals.Api.Mapping;
using ZDeals.Common;

namespace ZDeals.Api.Filters
{

    /// <summary>
    /// Convert the response in type Result<> to proper ActionResult
    /// </summary>
    public class ResponseFilter : IAsyncResultFilter
    {
        private readonly ILogger<ResponseFilter> _logger; 

        public ResponseFilter(ILogger<ResponseFilter> logger)
        {
            _logger = logger;
        }

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
                        if (resultValue.HasError(ErrorType.Internal))
                        {
                            status = StatusCodes.Status500InternalServerError;
                        }
                        else if (resultValue.HasError(ErrorType.NotFound))
                        {
                            status = StatusCodes.Status404NotFound;
                        }
                        else
                        {
                            status = StatusCodes.Status400BadRequest;
                        }

                        result.StatusCode = status;
                        result.Value = resultValue.Errors.ToErrorResponse(status);
                    }
                    else
                    {
                        var dataProperty = type.GetProperty("Data");
                        if (dataProperty != null)
                        {
                            var data = dataProperty.GetValue(resultValue);
                            result.Value = data;
                            if(data == null)
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
