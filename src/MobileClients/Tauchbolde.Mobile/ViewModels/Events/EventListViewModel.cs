using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Tauchbolde.Mobile.Models.Events;
using Tauchbolde.Mobile.Services;
using Xamarin.Forms;

namespace Tauchbolde.Mobile.ViewModels.Events
{
    public class EventListsViewModel : BaseViewModel
    {
        public IDataStore<EventListRow> DataStore => DependencyService.Get<IDataStore<EventListRow>>();
        public ObservableCollection<EventListRow> Rows { get; set;  }
        public Command LoadItemsCommand { get; set; }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public EventListsViewModel()
        {
            Title = "Aktivitäten";
            Rows = new ObservableCollection<EventListRow>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            IsBusy = false;

//            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
//            {
//                var newItem = item as Item;
//                Rows.Add(newItem);
//                await DataStore.AddItemAsync(newItem);
//            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Rows.Clear();
                var eventListRows = await DataStore.GetItemsAsync(true);
                foreach (var eventListRow in eventListRows)
                {
                    Rows.Add(eventListRow);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}