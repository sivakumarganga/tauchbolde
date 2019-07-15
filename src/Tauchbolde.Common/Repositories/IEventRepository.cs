﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Entities;

namespace Tauchbolde.Common.Repositories
{
    public interface IEventRepository: IRepository<Event>
    {
        /// <summary>
        /// Gets the list of upcoming event without the deleted
        /// and canceled events.
        /// </summary>
        /// <returns>All upcoming events</returns>
        Task<List<Event>> GetUpcomingEventsAsync();

        Task<ICollection<Event>> GetUpcomingAndRecentEventsAsync();
    }
}
