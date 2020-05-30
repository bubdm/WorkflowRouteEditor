using System.Collections.Generic;
using System.Linq;
namespace WorkflowRouteEditor.Entities
{
    public class Route : IRoute
    {
        public Route()
        {
            Next = new List<IRoute>();
        }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public List<IRoute> Next { get; set; }
    }
}
