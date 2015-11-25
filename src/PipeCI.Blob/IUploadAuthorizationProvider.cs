using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipeCI.Blob
{
    public interface IUploadAuthorizationProvider
    {
        bool IsAbleToUpload();
    }
}
