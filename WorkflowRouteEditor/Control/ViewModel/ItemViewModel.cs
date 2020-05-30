using WorkflowRouteEditor.Control.Common;

namespace WorkflowRouteEditor.Control.ViewModel
{
    internal class ItemViewModel<TEntity> : ViewModelBase
    { 
        public ItemViewModel()
        {
            Items = new ItemsCollection<TEntity>();
        }
        public ItemsCollection<TEntity> Items { get; }

    }
}
