using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkflowRouteEditor.Control.Repository
{
    internal interface IRouteItemRepository
    {
        Task<IEnumerable<RouteItem>> GetItemsAsync(string fileName);
        Task SaveItemsAsync(IEnumerable<RouteItem> items, string fileName);
        
    }
}
