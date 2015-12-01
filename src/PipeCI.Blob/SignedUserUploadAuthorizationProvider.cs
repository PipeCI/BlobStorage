using System.Linq;
using Microsoft.AspNet.Http;
using PipeCI.Blob;

namespace PipeCI.Blob
{
    public class SignedUserUploadAuthorizationProvider : IUploadAuthorizationProvider
    {
        protected HttpContext httpContext { get; set; }

        public SignedUserUploadAuthorizationProvider (IHttpContextAccessor accessor)
        {
            httpContext = accessor.HttpContext;
        }

        public bool IsAbleToUpload()
        {
            return httpContext.User.Identities.Count() > 0;
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SignedUserUploadAuthorizationProviderServiceCollectionExtensions
    {
        public static IBlobBuilder AddSignedUserBlobUploadAuthorization(this IBlobBuilder self)
        {
            self.Services.AddSingleton<IUploadAuthorizationProvider, SignedUserUploadAuthorizationProvider>();
            return self;
        }
    }
}