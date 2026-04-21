using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Garmetix.Core.ViewModels;
using Garmetix.UI.Services;

namespace Garmetix.UI.ViewModels
{
    [QueryProperty(nameof(EntityType), "EntityType")]
    [QueryProperty(nameof(EntityId), "EntityId")]
    public partial class DynamicEntryViewModel : BaseViewModel
    {
        private readonly MasterDataService _masterDataService;

        [ObservableProperty] private string _entityType;
        [ObservableProperty] private string _entityId;
        
        // This holds the dynamic object (Party, Bank, etc.)
        [ObservableProperty] private object _currentEntity;

        public DynamicEntryViewModel(MasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        partial void OnEntityTypeChanged(string value)
        {
            Title = $"Edit {value}";
            LoadEntity();
        }

        private async void LoadEntity()
        {
            if (string.IsNullOrEmpty(EntityId))
            {
                // New Entity
                CurrentEntity = _masterDataService.CreateNewEntity(EntityType);
            }
            else
            {
                // Editing existing: Fetch all, find by ID (Can be optimized later)
                var allData = await _masterDataService.GetAllAsync(EntityType);
                CurrentEntity = allData.FirstOrDefault(x => (x as Garmetix.Core.Models.BaseEntity).Id.ToString() == EntityId);
            }
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            IsBusy = true;
            try
            {
                bool isNew = string.IsNullOrEmpty(EntityId);
                await _masterDataService.SaveAsync(EntityType, CurrentEntity, isNew);
                
                await Application.Current.MainPage.DisplayAlert("Success", "Record saved securely.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex) { await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK"); }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        public async Task CancelAsync() => await Shell.Current.GoToAsync("..");
    }
}