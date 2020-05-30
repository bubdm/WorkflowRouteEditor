using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WorkflowRouteEditor.Control.Common
{
    public class ItemWraper<TEntity> : INotifyPropertyChanged 
        
    {
        protected ItemWraper(TEntity model)
        {
            Model = model;
        }
        public TEntity Model { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        protected virtual void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = null)
        {
            typeof(TEntity).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged(propertyName);
        }
        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            return (TValue)typeof(TEntity).GetProperty(propertyName).GetValue(Model);
        }
    }
}
