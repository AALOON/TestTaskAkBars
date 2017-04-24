using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using MobileApi.Server.Core;

namespace MobileApi.Server.Infrastructure
{
    class ApplicationIdentityResult : IIdentityResult
    {
        private readonly IdentityResult _result;
        public ApplicationIdentityResult(IdentityResult result)
        {
            _result = result;
        }

        public bool Succeeded => _result.Succeeded;

        public IEnumerable<string> Errors => _result.Errors;
    }
}
