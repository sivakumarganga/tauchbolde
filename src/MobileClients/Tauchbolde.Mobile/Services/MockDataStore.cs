using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tauchbolde.Mobile.Models.Events;

namespace Tauchbolde.Mobile.Services
{
    public class EventListRowMockStore : IDataStore<EventListRow>
    {
        readonly List<EventListRow> rows;

        public EventListRowMockStore()
        {
            rows = new List<EventListRow>()
            {
                new EventListRow {
                    EventId = new Guid("813DBB5B-4D78-4DAF-AA42-1E74DD34E5E9"),
                    Title = "Fun-Dive",
                    Location = "Zürichsee, Terlinden",
                    MeetingPoint = "5 Minutes before on site",
                    StartTime = new DateTime(2019, 10, 10, 19, 0, 0),
                    EndTime = null,
                }
            };
        }

        public async Task<bool> AddItemAsync(EventListRow item)
        {
            rows.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(EventListRow item)
        {
            var oldItem = rows.FirstOrDefault(arg => arg.EventId == item.EventId);
            rows.Remove(oldItem);
            rows.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            var oldItem = rows.FirstOrDefault(r => r.EventId == id);
            rows.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<EventListRow> GetItemAsync(Guid id)
        {
            return await Task.FromResult(rows.FirstOrDefault(s => s.EventId == id));
        }

        public async Task<IEnumerable<EventListRow>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(rows);
        }
    }
}