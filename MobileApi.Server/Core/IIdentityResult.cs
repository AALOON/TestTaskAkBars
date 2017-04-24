using System.Collections.Generic;

namespace MobileApi.Server.Core
{
    public interface IIdentityResult
    {
        bool Succeeded { get; }

        IEnumerable<string> Errors { get; }
    }
}
