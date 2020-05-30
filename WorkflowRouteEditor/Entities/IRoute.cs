using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowRouteEditor.Entities
{
    public interface IRoute
    {
        string Name { get; set; }
        string ClassName { get; set; }
        List<IRoute> Next { get; set; }
    }
}
