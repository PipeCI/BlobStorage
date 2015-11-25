using System;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.DependencyInjection;
using PipeCI.Blob;
using PipeCI.Blob.Middlewares;

namespace Microsoft.AspNet.Builder
{
    public static class Blob
    {
        public static IApplicationBuilder UseBlob(this IApplicationBuilder self)
        {
            var endpoint = new DelegateRouteEndpoint(async (context) => {
                var bs = context.HttpContext.RequestServices.GetRequiredService<IBlobProvider>();
                var id = Guid.Parse(context.RouteData.Values["id"].ToString());
                var blob = bs.Get(id);
                if (blob == null)
                {
                    context.HttpContext.Response.StatusCode = 404;
                    await context.HttpContext.Response.WriteAsync("Not Found");
                }
                else
                {
                    context.HttpContext.Response.ContentType = blob.ContentType;
                    context.HttpContext.Response.ContentLength = blob.ContentLength;
                    context.HttpContext.Response.Headers["Content-disposition"] = $"attachment; filename={WebUtility.UrlEncode(blob.FileName)}";
                    context.HttpContext.Response.Body.Write(blob.File, 0, blob.File.Length);
                }
            });
            var routeBuilder = new RouteBuilder();
            routeBuilder.ServiceProvider = self.ApplicationServices;
            routeBuilder.DefaultHandler = endpoint;
            routeBuilder.MapRoute("DownBlob", "blob/download/{id:Guid}");
            return self.UseRouter(routeBuilder.Build())
                .Map("/blob", a => {
                a.Map("/upload", b =>
                {
                    b.Run(async c =>
                    {
                        var auth = self.ApplicationServices.GetRequiredService<IUploadAuthorizationProvider>();
                        if (!auth.IsAbleToUpload())
                        {
                            c.Response.StatusCode = 403;
                            await c.Response.WriteAsync("Forbidden");
                        }
                        else if (c.Request.Method == "POST")
                        {
                            var bs = self.ApplicationServices.GetRequiredService<IBlobProvider>();
                            var file = c.Request.Form.Files["file"];
                            if (file != null)
                            {
                                var id = bs.Set(new PipeCI.Blob.Models.Blob
                                {
                                    Time = DateTime.Now,
                                    ContentType = file.ContentType,
                                    ContentLength = file.Length,
                                    FileName = file.GetFileName(),
                                    File = file.ReadAllBytes()
                                });
                                await c.Response.WriteAsync(id.ToString());
                            }
                            else
                            {
                                var img = new Base64StringImage(c.Request.Form["file"]);
                                var id = bs.Set(new PipeCI.Blob.Models.Blob
                                {
                                    Time = DateTime.Now,
                                    ContentType = img.ContentType,
                                    ContentLength = img.ImageString.Length,
                                    FileName = "file",
                                    File = img.AllBytes
                                });
                                await c.Response.WriteAsync(id.ToString());
                            }
                        }
                        else
                        {
                            c.Response.StatusCode = 400;
                            await c.Response.WriteAsync("Bad Request");
                        }
                    });
                });
            });
        }
    }
}
