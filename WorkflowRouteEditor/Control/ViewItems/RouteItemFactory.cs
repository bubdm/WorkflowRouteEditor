using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.Control.ViewItems
{
    internal class RouteItemFactory : IRouteItemFactory
    {
        private static IRouteItemFactory _instance = new RouteItemFactory();
        public static IRouteItemFactory Instance { get => _instance; }
        public IEnumerable<RouteItem> Create(IEnumerable<IRoute> values)
        {
            var items = values.ToDictionary(k=> k.Name, r => new RouteItem(r)
            {
                Width = 100,
                Height = 100,
                FillColor = Colors.Navy,
                BorderColor = Colors.Red,
                FontColor = Colors.White
            });

            items.Values.ToList().ForEach((k) =>
            {
                using (k.Links.LockChangedEvent())
                {
                    k.Links.AddRange(k.Model.Next.Select(r => new LinkItem(k, items[r.Name])));
                }
            });
            
            return items.Values.ToArray();
        }

    }
}
