using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Common.Model;

namespace Tauchbolde.Common.DomainServices.Logbook
{
    /// <summary>
    /// Domain services for the logbook functionality.
    /// </summary>
    public interface ILogbookService
    {
        /// <summary>
        /// Gets the collection of all <see cref="LogbookEntry"/>.
        /// </summary>
        Task<ICollection<LogbookEntry>> GetAllEntriesAsync();

        /// <summary>
        /// Gets the <see cref="LogbookEntry"/> by its ID.
        /// </summary>
        /// <param name="logbookEntryId">The ID of the logbook entry to get.</param>
        /// <returns>The <see cref="LogbookEntry"/>.</returns>
        Task<LogbookEntry> FindByIdAsync(Guid logbookEntryId);

        /// <summary>
        /// Update or insert an entry into the logbook.
        /// </summary>
        /// <param name="model">The logbook model to update or insert.</param>
        /// <returns>The ID of the logbook entry.</returns>
        Task<Guid> UpsertAsync(LogbookUpsertModel model);
    }
}