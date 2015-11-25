using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipeCI.Blob
{
    public class AnonymousUploadAuthorizationProvider : IUploadAuthorizationProvider
    {
        public bool IsAbleToUpload()
        {
            return true;
        }
    }
}
