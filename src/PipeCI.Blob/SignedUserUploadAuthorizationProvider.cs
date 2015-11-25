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
            if (httpContext.User.Identity.IsAuthenticated)
                return true;
            else
                return false;
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