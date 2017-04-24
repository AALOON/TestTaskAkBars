using System;
using MobileApi.Server.Core;
using MobileApi.Server.Core.Repositories;
using MobileApi.Server.Infrastructure.Repositories;

namespace MobileApi.Server.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Identity = new IdentityRepository();
            UserProfile = new UserProfileRepository(_context);
        }

        public IIdentityRepository Identity { get; private set; }
        public IUserProfileRepository UserProfile { get; }

        public int Save()
        {
            return _context.SaveChanges();
        }

        #region < IDisposable >

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
