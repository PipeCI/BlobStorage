using System;

namespace PipeCI.Blob
{
    public interface IBlobProvider
    {
        Models.Blob Get(Guid id);
        void Delete(Guid id);
        Guid Set(Models.Blob blob);
    }
}
