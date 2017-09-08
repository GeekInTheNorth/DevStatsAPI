using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace DevStats.Domain.Security
{
    public interface IApplicationUserStore<TUser, in TKey> : IUserEmailStore<TUser, TKey>, IUserPasswordStore<TUser, TKey>, IUserLockoutStore<TUser, TKey>, IUserTwoFactorStore<TUser, TKey>, IUserSecurityStampStore<TUser, TKey>, IDisposable where TUser : class, IUser<TKey>
    {
        IEnumerable<TUser> GetUsers();
    }
}