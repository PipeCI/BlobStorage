using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Moq;

namespace PipeCI.Blob.Tests
{
    public class AnonymousUploadAuthorizationProviderTests
    {
        [Fact]
        public void not_signed_in()
        {
            // Arrange
            var user = new Mock<ClaimsPrincipal>();
            user.Setup(x => x.Claims)
                .Returns(new List<Claim>());

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.User)
                .Returns(user.Object);

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddScoped<IHttpContextAccessor>(x => httpContextAccessor.Object);
            collection.AddBlob();

            var services = collection.BuildServiceProvider();
            var uap = services.GetRequiredService<IUploadAuthorizationProvider>();

            // Act
            var result = uap.IsAbleToUpload();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void already_signed_in()
        {
            // Arrange
            var user = new Mock<ClaimsPrincipal>();
            user.Setup(x => x.Identities)
                .Returns(new List<ClaimsIdentity> { new ClaimsIdentity() });

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.User)
                .Returns(user.Object);

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext)
                .Returns(httpContext.Object);

            var collection = new ServiceCollection();
            collection.AddScoped<IHttpContextAccessor>(x => httpContextAccessor.Object);
            collection.AddBlob();

            var services = collection.BuildServiceProvider();
            var uap = services.GetRequiredService<IUploadAuthorizationProvider>();

            // Act
            var result = uap.IsAbleToUpload();

            // Assert
            Assert.True(result);
        }

    }
}
