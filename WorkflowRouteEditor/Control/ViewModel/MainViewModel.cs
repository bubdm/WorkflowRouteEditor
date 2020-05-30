using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowRouteEditor.Control.Common;
using WorkflowRouteEditor.Control.Repository;
using WorkflowRouteEditor.Control.ViewItems;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.Control.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        public static MainViewModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MainViewModel(
                        new RouteItemViewModel(new RouteItemRepository(), RouteItemFactory.Instance),
                        new RouteViewModel());
                }

                return _Instance;
            }
        }
        private readonly RouteItemViewModel _routeItemViewModel;
        private readonly RouteViewModel _routeViewModel;
        private static MainViewModel _Instance;

        protected MainViewModel(
            RouteItemViewModel routeItemViewModel,
            RouteViewModel routeViewModel
            )
        {
            _routeItemViewModel = routeItemViewModel;
            _routeViewModel = routeViewModel;
        }

        public void LoadItems(string fileName)
        {
            _routeItemViewModel.LoadFromFile(fileName);
        }

        public void LoadItems(IEnumerable<IRoute> routes)
        {
            _routeItemViewModel.LoadFromSource(routes);
        }

        public RouteItemViewModel RouteItemViewModel
        {
            get => _routeItemViewModel;
        }

        public RouteViewModel RouteViewModel
        {
            get => _routeViewModel;
        }
    }
}
