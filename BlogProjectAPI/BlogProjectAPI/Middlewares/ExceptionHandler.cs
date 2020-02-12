using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BlogProjectAPI.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;
        public ExceptionHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            //if (ex is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (ex is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (ex is MyException) code = HttpStatusCode.BadRequest;

            var st = new StackTrace(ex, true);
            string[] frame = new string[10];
            for (int i = 0; i < 10; i++)
            {
                var f = st.GetFrame(i).GetFileName();
                if (!string.IsNullOrEmpty(f))
                {
                    frame[i] += f;
                }
            }

            var result = JsonConvert.SerializeObject(
                new { Stacktrace = frame, Src = ex.Source, error = ex.Message },
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
