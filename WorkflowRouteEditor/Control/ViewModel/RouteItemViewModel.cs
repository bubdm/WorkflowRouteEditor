using System;
using System.Collections.Generic;
using System.Windows.Controls;
using WorkflowRouteEditor.Control.Repository;
using WorkflowRouteEditor.Control.ViewItems;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.Control.ViewModel
{
    internal class RouteItemViewModel : ItemViewModel<RouteItem>
    {
        private readonly IRouteItemRepository _repository;
        private readonly IRouteItemFactory _factory;
        private RouteItem _selectedItem;

        public RouteItemViewModel(IRouteItemRepository repository, IRouteItemFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public RouteItem SelectedItem 
        { 
            get => _selectedItem; 
            set
            {
                _selectedItem = value;
                
                OnPropertyChanged();
            }
        }
        public async void LoadFromFile(string value)
        {
            LoadItems(await _repository.GetItemsAsync(value));
        }
        public void LoadFromSource(IEnumerable<IRoute> source)
        {
            LoadItems(_factory.Create(source));
        }
        private void LoadItems(IEnumerable<RouteItem> values)
        {
            using (Items.LockChangedEvent())
            {
                Items.Clear();

                Items.AddRange(values);
            }

            //OnPropertyChanged("Items");

            if(Items.Count > 0) SelectedItem = Items[0];
        }
        
    }
}
