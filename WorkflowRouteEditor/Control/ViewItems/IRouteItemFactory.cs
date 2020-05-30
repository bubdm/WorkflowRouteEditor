using System.Collections.Generic;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.Control.ViewItems
{
    interface IRouteItemFactory
    {
        IEnumerable<RouteItem> Create(IEnumerable<IRoute> values);
    }
}
