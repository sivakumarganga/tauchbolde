using System;
namespace Tauchbolde.Mobile.Models.Events
{
    public class EventListRow
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string MeetingPoint { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
