using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PipeCI.Blob;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlobServiceCollectionExtensions
    {
        public static IBlobBuilder AddBlob(this IServiceCollection self)
        {
            var builder = new BlobBuilder();
            builder.Services = self.AddRouting();
            builder.Services.AddScoped<IUploadAuthorizationProvider, AnonymousUploadAuthorizationProvider>();
            return builder;
        }
    }
}
