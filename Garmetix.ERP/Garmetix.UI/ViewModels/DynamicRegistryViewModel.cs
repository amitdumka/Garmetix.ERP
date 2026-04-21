using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Garmetix.Core.Interfaces;
using Garmetix.Core.ViewModels;
using Garmetix.UI.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace Garmetix.UI.ViewModels
{
    [QueryProperty(nameof(EntityType), "EntityType")]
    public partial class DynamicRegistryViewModel : BaseViewModel
    {
        private readonly MasterDataService _masterDataService;

        [ObservableProperty] private string _entityType; // e.g., "Party", "Bank"
        [ObservableProperty] private ObservableCollection<object> _items = new();

        public DynamicRegistryViewModel(MasterDataService masterDataService, INotificationService notification) : base(notification)
        {
            _masterDataService = masterDataService;
        }

        partial void OnEntityTypeChanged(string value)
        {
            Title = $"{value} Directory";
            _ = LoadDataAsync();
        }

        [RelayCommand]
        public async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var data = await _masterDataService.GetAllAsync(EntityType);
                Items = new ObservableCollection<object>(data);
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        public async Task AddNewAsync()
        {
            // Pass the entity type and signal it is a NEW record
            await Shell.Current.GoToAsync($"DynamicEntryPage?EntityType={EntityType}");
        }

        [RelayCommand]
        public async Task ActionMenuAsync(object entity)
        {
            if (entity == null) return;

            string action = await Application.Current.MainPage.DisplayActionSheet("Record Options", "Cancel", "Delete", "Edit Record");

            if (action == "Edit Record")
            {
                // Serialize the object ID to pass it to the edit page
                var baseEntity = entity as Garmetix.Core.Models.BaseEntity;
                await Shell.Current.GoToAsync($"DynamicEntryPage?EntityType={EntityType}&EntityId={baseEntity.Id}");
            }
            else if (action == "Delete")
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Delete this record?", "Yes", "No");
                if (confirm)
                {
                    await _masterDataService.DeleteAsync(EntityType, entity);
                    await LoadDataAsync();
                }
            }
        }
    }
}