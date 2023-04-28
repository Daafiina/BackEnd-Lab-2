using Microsoft.AspNetCore.Mvc.Filters;

namespace BmmAPI.Filters
{
    public class MyExceptionFilter:ExceptionFilterAttribute
    {
        private readonly ILogger<MyExceptionFilter> logger;

        public MyExceptionFilter(ILogger<MyExceptionFilter> logger) 
        {
            this.logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            

            logger.LogError(context.Exception, context.Exception.Message);

            base.OnException(context);
        }
    }
}
