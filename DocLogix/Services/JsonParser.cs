using DocLogix.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DocLogix.Services
{
    public class JsonParser
    {
        public JsonParser() { }

        public void PrepareResultsJson(List<Device> results, string query, int logCount)
        {

            var dictionary = new Dictionary<string, object>
            {
                { "query", query },
                { "result", results },
                { "logCount", logCount }
            };

            // Serialize the dictionary to JSON
            var json = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
            Console.WriteLine(json.ToString());
        }
    }
}
