using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocLogix.Services;

namespace DocLogix
{
    public class UI
    {
        public UI()
        {

        }

        public void DisplayEntryMessage()
        {
            Console.WriteLine("Upload a file from local system (paste a path to the file, for example C:\\TadasPC\\Downloads\\15151615.csv): ");
        }
        public string GetFilePath()
        {
            return Console.ReadLine();
        }
        public void DoUI()
        {
            SearchEngine searchEngine = new SearchEngine();
            JsonParser jp = new JsonParser();
            FileOpener fo = new FileOpener();

            DisplayEntryMessage();
            string path = GetFilePath();

            //store list<device> in the fileopener fo
            fo.StoreFileDataToList(path);
            //if empty, end program
            if (fo.DevicesFromFile.Count == 0)
            {
                Console.WriteLine("empty file bye");
                return;
            }

            Console.WriteLine("\n! ! ! YOU CAN USE LOGICAL OPERATORS FOR QUERIES SUCH AS || && ^^. FOR EXAMPLE deviceVendor = 'TEXT' && deviceProduct 'TEXT2' ! ! ! \n ");
            string query = string.Empty;
            while (!query.Equals("0"))
            {
                Console.WriteLine("\nEnter a query:");
                query = Console.ReadLine();
                var results = searchEngine.Search(query, fo.DevicesFromFile);
                Console.WriteLine();
                jp.PrepareResultsJson(results, query, results.Count);
                Console.WriteLine();

                
            }
        }


    }
}
