using System;
using System.Linq;
using Microsoft.Data.Entity;
using PipeCI.Blob;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EFBlobProviderServiceCollectionExtensions
    {
        public static IBlobBuilder AddEntityFrameworkStorage<TContext>(this IBlobBuilder self)
            where TContext : DbContext, IBlobDbContext
        {
            self.Services.AddScoped<IBlobProvider, EFBlobProvider<TContext>>();
            self.Services.AddScoped<IUploadAuthorizationProvider, AnymouseUploadAuthorizationProvider>();
            return self;
        }
    }
}

namespace PipeCI.Blob
{
    public class EFBlobProvider<TContext> : IBlobProvider
        where TContext : DbContext, IBlobDbContext
    {
        protected IBlobDbContext DbContext { get; set; }

        public EFBlobProvider(TContext db)
        {
            DbContext = db;
        }

        public void Delete(Guid id)
        {
            var blob = DbContext.Blobs.Where(x => x.Id == id).SingleOrDefault();
            if (blob != null)
            {
                DbContext.Blobs.Remove(blob);
                DbContext.SaveChanges();
            }
        }

        public Models.Blob Get(Guid id)
        {
            return DbContext.Blobs.Where(x => x.Id == id).SingleOrDefault(); 
        }

        public Guid Set(Models.Blob blob)
        {
            if (blob.Id != default(Guid) && DbContext.Blobs.Where(x => x.Id == blob.Id).SingleOrDefault() != null)
                Delete(blob.Id);
            DbContext.Blobs.Add(blob);
            DbContext.SaveChanges();
            return blob.Id;
        }
    }
}
