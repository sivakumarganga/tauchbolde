using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tauchbolde.Mobile.Application.Services.Communication;
using Tauchbolde.Mobile.Models.Events;
using Tauchbolde.Mobile.Services;

namespace Tauchbolde.Mobile.Application.Services
{
    public class EventsRestDataStore : IDataStore<EventListRow>
    {
        private readonly IRestService restService;

        public EventsRestDataStore(IRestService restService)
        {
            this.restService = restService;
        }

        public Task<bool> AddItemAsync(EventListRow item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<EventListRow> GetItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EventListRow>> GetItemsAsync(bool forceRefresh = false)
        {
            var url = "";
            var result = await restService.GetAsync<EventListRow>(url);

            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(EventListRow item)
        {
            throw new NotImplementedException();
        }
    }
}
