using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Tauchbolde.Mobile.Models.Events
{
    public class EventListRow : INotifyPropertyChanged
    {
        [JsonProperty("eventId")]
        public Guid EventId { get; set; }

        private string title;
        [JsonProperty("title")]
        public string Title {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string location;
        [JsonProperty("location")]
        public string Location {
            get => location;
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged();
                }
            }
        }

        private string meetingPoint;
        [JsonProperty("meetingPoint")]
        public string MeetingPoint {
            get => meetingPoint;
            set
            {
                if (meetingPoint != value)
                {
                    meetingPoint = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime startTime;
        [JsonProperty("startTime")]
        public DateTime StartTime {
            get => startTime;
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime? endTime;
        [JsonProperty("endTime")]
        public DateTime? EndTime {
            get => endTime;
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
