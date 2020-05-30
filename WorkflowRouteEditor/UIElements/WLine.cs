using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using WorkflowRouteEditor.Control;
using WorkflowRouteEditor.Entities;

namespace WorkflowRouteEditor.WUIElements
{
    internal class WLine : WUIElement<Link>
    {
        public WLine(VisualHostContainer parent, Link item) :
            base(parent, item)
        {
            
        }

        public Point Start { get; set; }
        public Point End { get; set; }
        
        
    }
}
