using System.Linq;
using WorkflowRouteEditor.Control.ViewItems;

namespace WorkflowRouteEditor.Control
{
    internal static class ItemsExtentions
    {
        public static bool CanRedirected(this LinkItem link, RouteItem To)
        {
            return link.From != To && link.From.Links.Any(li => li.To == To) == false;
        }
        public static void RedirectLink(this LinkItem link, RouteItem To)
        {
            if (link.CanRedirected(To) == false) return;

            link.To = To;
        }

        public static LinkItem CreateLink(this RouteItem From, RouteItem To)
        {
            var link = new LinkItem(From, To);
            From.Links.Add(link);

            return link;
        }

        public static void DeleteLink(this LinkItem link)
        {
            link.From.DeleteLink(link); 
        }

        public static void DeleteLink(this RouteItem From, LinkItem link)
        {
            From.Links.Remove(link);
        }
    }
}
