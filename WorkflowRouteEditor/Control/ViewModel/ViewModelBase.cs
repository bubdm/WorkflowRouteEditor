using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WorkflowRouteEditor.Control.ViewModel
{
    internal  class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
