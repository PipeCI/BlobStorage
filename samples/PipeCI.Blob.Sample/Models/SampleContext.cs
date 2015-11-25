using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using PipeCI.Blob.Models;

namespace PipeCI.Blob.Sample.Models
{
    public class SampleContext : DbContext, IBlobDbContext
    {
        public DbSet<Blob.Models.Blob> Blobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SetupBlob();
        }
    }
}
