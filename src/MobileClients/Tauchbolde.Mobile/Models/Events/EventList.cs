using System.Collections.Generic;

namespace Tauchbolde.Mobile.Models.Events
{
    public class EventList
    {
        public ICollection<EventListRow> Rows { get; set; }
    }
}