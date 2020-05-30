using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace WorkflowRouteEditor.Control.Repository
{
    internal class RouteItemRepository : IRouteItemRepository
    {
        public RouteItemRepository()
        {
        }
        public async Task<IEnumerable<RouteItem>> GetItemsAsync(string fileName)
        {
            string filepath = GetFilePath(fileName);
            using (var reader = File.OpenText(filepath))
            {
                var jo = await JObject.LoadAsync(new JsonTextReader(reader as TextReader));
                return jo.ToObject<IEnumerable<RouteItem>>();
            }
        }
        public async Task SaveItemsAsync(IEnumerable<RouteItem> items, string fileName)
        {
            string filepath = GetFilePath(fileName);
            var text = JsonConvert.SerializeObject(items);
            using (var writer = File.CreateText(filepath))
            {
                await writer.WriteAsync(text);
            }
        }
        private string GetFilePath(string filename)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), filename);
        }
    }
}
