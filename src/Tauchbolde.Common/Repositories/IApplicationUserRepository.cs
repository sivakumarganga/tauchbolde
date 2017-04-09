﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;
using Tauchbolde.Common.Repository;

namespace Tauchbolde.Common.Repositories
{
    public interface IApplicationUserRepository: IRepository<ApplicationUser>
    {
        /// <summary>
        /// Finds a <see cref="ApplicationUser"/> by its username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The user if found; otherwise <c>Null</c> will be returned.</returns>
        Task<ApplicationUser> FindByUserNameAsync(string username);

        /// <summary>
        /// Get a collection of all users that are members of Tauchbolde.
        /// </summary>
        /// <returns>A collection of all users that are members of Tauchbolde.</returns>
        Task<ICollection<ApplicationUser>> GetAllTauchboldeUsersAsync();
    }
}
