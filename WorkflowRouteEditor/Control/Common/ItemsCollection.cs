using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WorkflowRouteEditor.Control.Common
{
    public class ItemsCollection<TEntity> : ObservableCollection<TEntity>
    {
        private class Locker : IDisposable
        {
            private readonly ItemsCollection<TEntity> _collection;
            public Locker(ItemsCollection<TEntity> collection)
            {
                _collection = collection;
                _collection.LockRaiseEvent = true;
            }
            public void Dispose()
            {
                _collection.LockRaiseEvent = false;
                _collection.RaiseCollectionChanged();
            }
        }

        private bool LockRaiseEvent { get; set; }
        private void RaiseCollectionChanged()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public ItemsCollection() : base()
        {

        }
        public ItemsCollection(IEnumerable<TEntity> collection) :
            base(collection)
        {
        }
        public void AddRange(IEnumerable<TEntity> collection)
        {
            if (collection == null) return;

            using (IEnumerator<TEntity> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    this.Add(enumerator.Current);
                }
            }
        }
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (LockRaiseEvent) return;
            base.OnCollectionChanged(e);
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (LockRaiseEvent) return;
            base.OnPropertyChanged(e);
        }
        public IDisposable LockChangedEvent()
        {
            return new Locker(this);
        }

    }
}
