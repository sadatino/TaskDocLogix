using DocLogix.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocLogix.Services
{
    public class SearchEngine
    {
        // you could expand this with additional string logical operators, and add an if statement in DoLogicalOperation() to expand functionality
        private readonly string[] substrings = { "^^", "||", "&&" };

        public SearchEngine()
        {

        }

        public List<Device> Search(string query, List<Device> list)
        {
            // get the property/column names
            PropertyInfo[] propertyInfo = GetPropertyInfoOfDevice();

            string[] propNames = GetPropertyNames(propertyInfo);

            List<Device> resultList = new List<Device>();

            resultList = DoSearchWithEveryQuery(query, propNames, list,propertyInfo);

            return resultList;

        }
        private List<Device> DoSearchWithEveryQuery(string query, string[] propNames, List<Device> list, PropertyInfo[] propertyInfo)
        {
            //get a dictionary where key = occurence in the whole query and value is the logical operator
            var orderOfOperations = GetOperationOrderFromQuery(query);
            //split the big query in subqueries when logical operators are met in the big query
            var queries = SplitStringWhenOperationFound(query);

            //create a result list and the searchable querys list
            var resultList = new List<Device>();
            List<Device> searchResults = new List<Device>();

            // means it will only be one query, call ParseQueryAndSearch to see if it has column and text to search
            if (orderOfOperations.Count == 0)
            {
                searchResults = ParseQueryAndSearch(propertyInfo, propNames, query, list);
                return searchResults;
            }
            //else there will be logical operators
            else
            {
                int i = 0;
                foreach (var q in queries)
                {
                    // if i = 0, does the first query in substring of queries
                    if (i == 0)
                    {
                        searchResults = ParseQueryAndSearch(propertyInfo, propNames, q, list);
                        resultList = DoLogicalOperation(String.Empty, searchResults, resultList);

                    }
                    //else does it with every left q in queries
                    else
                    {
                        searchResults = ParseQueryAndSearch(propertyInfo, propNames, q, list);
                        resultList = DoLogicalOperation(orderOfOperations[i], searchResults, resultList);
                    }
                    i++;
                }
            }
            return resultList;
        }
        private List<Device> DoLogicalOperation(string logicalOperation, List<Device> searchResults, List<Device> resultList)
        {
            //OR Logic
            if (logicalOperation.Equals("||"))
            {
                resultList = resultList.Union(searchResults).ToList();
            }
            //AND logic
            else if (logicalOperation.Equals("&&"))
            {
                resultList = resultList.Intersect(searchResults).ToList();
            }
            //XOR logic
            else if (logicalOperation.Equals("^^"))
            {
                resultList = resultList.Except(searchResults).ToList();
            }
            //if theres no more logical operators
            else
            {
                resultList = searchResults.ToList();
            }
            return resultList;

        }
        private Dictionary<int, string> GetOperationOrderFromQuery(string str)
        {

            // The dictionary to store the numbered substrings
            Dictionary<int, string> dict = new Dictionary<int, string>();

            // The current position in the string
            int pos = 0;

            // The current number
            int num = 1;

            // Iterate over the string and find the positions of the substrings
            while (pos < str.Length)
            {
                int index = -1;

                string substring = "";

                foreach (string s in substrings)
                {
                    int i = str.IndexOf(s, pos);
                    if (i != -1 && (index == -1 || i < index))
                    {
                        index = i;
                        substring = s;
                    }
                }

                // If a substring was found, add it to the dictionary with its number as the key
                if (index != -1)
                {
                    dict[num] = substring;

                    num++;
                    pos = index + substring.Length;
                }
                else
                {
                    // If no substring was found, break out of the loop
                    break;
                }
            }

            return dict;
        }
        private string[] SplitStringWhenOperationFound(string str)
        {
            return str.Split(substrings, StringSplitOptions.None);
        }
        private List<Device> SearchRecords(string textToSearch, string columnName, PropertyInfo[] propertyInfo, List<Device> list)
        {
            // search for x in column y

            PropertyInfo searchFor = null;

            foreach (PropertyInfo property in propertyInfo)
            {
                if (columnName.Equals(property.Name.ToLower()))
                    searchFor = property;
            }
            List<Device> result = new List<Device>();
            foreach (Device device in list)
            {
                if (searchFor.GetValue(device).ToString().ToLower().Contains(textToSearch.ToLower()))
                {
                    result.Add(device);
                }
            }
            return result;
        }
        private string GetTextToSearch(string query)
        {
            // returns empty string if theres no text to search
            if (string.IsNullOrEmpty(Regex.Match(query, "\'(.*?)\'").Groups[1].Value))
                return string.Empty;
            //returns text inside ' '
            return Regex.Match(query, "\'(.*?)\'").Groups[1].Value;
        }
        private string GetMatchedPropertyName(string[] propNames, string query)
        {
            // check if query has property name

            string[] words = query.ToLower().Split(new[] { "=", " ", "[", "]", "{", "}" }, StringSplitOptions.RemoveEmptyEntries);
            string[] result = words.Intersect(propNames).ToArray();

            // check if column exists 
            if (CheckIfColumnExists(result, query))
            {
                //returns column name
                return result[0];
            }
            return string.Empty;
        }
        private bool CheckIfColumnExists(string[] result, string query)
        {
            if (result.Length == 0)
            {
                Console.WriteLine("cant do this query '{0}' because theres no such column.", query);
                return false;
            }
            return true;
        }
        private string[] GetPropertyNames(PropertyInfo[] propertyInfo)
        {
            string[] propNames = new string[propertyInfo.Length];
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                propNames[i] = propertyInfo[i].Name.ToLower();
            }
            return propNames;
        }
        private List<Device> ParseQueryAndSearch(PropertyInfo[] propertyInfo, string[] propNames, string query, List<Device> list)
        {
            // get column name from query, if there is no column, function ends

            string result = GetMatchedPropertyName(propNames, query);
            if (string.IsNullOrEmpty(result))
            {
                return new List<Device> { };
            }

            // get the text for query, if none, function ends
            string textToSearch = GetTextToSearch(query);
            if (string.IsNullOrEmpty(textToSearch))
            {
                Console.WriteLine("There was no searchable text");
                return new List<Device> { };
            }

            // search for x in column y
            var resultFromQuery = SearchRecords(textToSearch, result, propertyInfo, list);

            return resultFromQuery;
        }
        private PropertyInfo[] GetPropertyInfoOfDevice()
        {
            // get the property/column names
            Type deviceType = typeof(Device);
            string fullClassName = deviceType.Namespace + "." + deviceType.Name;

            Type T = Type.GetType(fullClassName);
            PropertyInfo[] propertyInfo = T.GetProperties();

            return propertyInfo;
        }
    }
}
