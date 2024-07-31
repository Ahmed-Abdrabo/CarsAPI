using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace CarsAPI.Extensions;

public static class CustomExceptionExtension
{
    public static void HandleError(this IApplicationBuilder app, bool isDevelopment)
    {
        app.UseExceptionHandler(error =>
        {
            error.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                if (feature != null)
                {
                    if (isDevelopment)
                    {
                        if (feature.Error is BadImageFormatException badImageException)
                        {
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                            {
                                StatusCode = 776,
                                ErrorMessage = "Hello from Custom Handler! Image format is invalid",
                            }));
                        }
                        else
                        {
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                            {
                                StatusCode = context.Response.StatusCode,
                                ErrorMessage = feature.Error.Message,
                                StackTrace = feature.Error.StackTrace,
                            }));
                        }
                    }
                    else
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            StatusCode = context.Response.StatusCode,
                            ErrorMessage = "Hello from program.cs Exception Handler",
                        }));
                    }
                }
            });
        });
    }
}