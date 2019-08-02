using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tauchbolde.Application.DataGateways;
using Tauchbolde.Application.Services.PhotoStores;
using Tauchbolde.Domain.Entities;

namespace Tauchbolde.Application.OldDomainServices.Logbook
{
    /// <summary>
    /// Standard implementation of <see cref="ILogbookService"/>.
    /// </summary>
    internal class LogbookService : ILogbookService
    {
        [NotNull] private readonly ILogbookEntryRepository logbookEntryRepository;
        [NotNull] private readonly IPhotoService photoService;

        public LogbookService(
            [NotNull] ILogbookEntryRepository logbookEntryRepository,
            [NotNull] IPhotoService photoService)
        {
            this.logbookEntryRepository = logbookEntryRepository ?? throw new ArgumentNullException(nameof(logbookEntryRepository));
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        /// <inheritdoc />
        public async Task<LogbookEntry> FindByIdAsync(Guid logbookEntryId)
            => await logbookEntryRepository.FindByIdAsync(logbookEntryId);
    }
}