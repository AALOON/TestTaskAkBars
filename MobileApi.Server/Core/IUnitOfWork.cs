using System;
using MobileApi.Server.Core.Repositories;

namespace MobileApi.Server.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IIdentityRepository Identity { get; }
        IUserProfileRepository UserProfile { get; } 
        int Save();
    }
}