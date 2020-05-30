using System.Collections.ObjectModel;
using WorkflowRouteEditor.Control.Common;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.Entities
{
    public class RouteDataSource : ItemsCollection<Route>
    {
        public RouteDataSource()
        {
            Route rt1 = new Route { Name = "First" };
            Route rt2 = new Route { Name = "Second" };

            Route rt3 = new Route { Name = "Third" };
            Route rt4 = new Route { Name = "Fourth" };
            Route rt5 = new Route { Name = "Fith" };

            rt1.Next.AddRange(new Route[] { rt2, });
            rt2.Next.AddRange(new Route[] { rt1, rt3 });
            rt3.Next.AddRange(new Route[] { rt2, rt4 });
            rt4.Next.AddRange(new Route[] { rt3, rt5 });
            rt5.Next.AddRange(new Route[] { rt4, rt1 });
            this.AddRange(new Route[] { rt1, rt2, rt3, rt4, rt5 });
        }
    }
}
