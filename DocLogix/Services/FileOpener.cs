using DocLogix.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DocLogix.Services
{
    class FileOpener
    {
        private readonly string XML = ".xml";
        private readonly string CSV = ".csv";


        private List<Device> devicesFromFile = new List<Device>();

        public void StoreFileDataToList(string path)
        {
            var extensionActions = new Dictionary<string, Action>
            {
                    { XML , () => HandleXmlFile(path) },
                    { CSV , () => HandleCsvFile(path) },
                    // Add more entries here for other file types
            };

            string extension = Path.GetExtension(path);

            // Check if the dictionary contains an action for the given file extension
            if (extensionActions.ContainsKey(extension))
            {
                // If an action is defined, execute it
                extensionActions[extension].Invoke();
                Console.WriteLine("\nFile uploaded.\n");
            }
            else
            {
                Console.WriteLine("Cant support this type of file or file is empty. Bye bye!");
            }
        }

        private void HandleCsvFile(string path)
        {
            var devices = new List<Device>();
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {

                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    Device device = new Device(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7], fields[8], fields[9], fields[10], fields[11], fields[12], fields[13], fields[14], fields[15], fields[16], fields[17]);
                    devices.Add(device);
                }
            }
            devicesFromFile = devices;
        }
        private void HandleXmlFile(string path)
        {
            List<Device> deviceList = new List<Device>();
            XDocument xmlDoc = XDocument.Load(path);

            // Select all the <device> elements from the document
            IEnumerable<XElement> items = xmlDoc.Root.Elements("device");

            // Loop through each <device> element and print its attributes
            foreach (XElement item in items)
            {
                Device device = new Device(item.Element("deviceVendor").Value, item.Element("deviceProduct").Value, item.Element("deviceVersion").Value, item.Element("signatureId").Value,
                    item.Element("severity").Value, item.Element("name").Value, item.Element("start").Value, item.Element("rt").Value, item.Element("msg").Value, item.Element("shost").Value,
                    item.Element("smac").Value, item.Element("dhost").Value, item.Element("dmac").Value, item.Element("suser").Value, item.Element("suid").Value, item.Element("externalId").Value,
                    item.Element("cs1Label").Value, item.Element("cs1").Value);
                deviceList.Add(device);
            }
            devicesFromFile = deviceList;
        }
        public List<Device> DevicesFromFile { get => devicesFromFile; }
    }
}
