using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WorkflowRouteEditor.Control.Common;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.Control.ViewModel
{
    internal class RouteViewModel : ItemViewModel<IRoute>
    {
        public RouteViewModel()
        {
            LoadCommand = new ItemRelayCommand(OnLoadItem);
        }

        public ICommand LoadCommand { get; }

        private void OnLoadItem()
        {
            throw new NotImplementedException();
        }
    }
}
