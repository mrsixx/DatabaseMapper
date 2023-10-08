using DatabaseMapper.Graph.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DatabaseMapper.Graph
{
    public class StrutReader : IStrutReader
    {
        public IEnumerable<string> GetColumns(string filePath)
        {
            var columns = new List<string>();
            string xml = File.ReadAllText(filePath);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNodeList tableNodes = xmlDoc.SelectNodes("//table");

            foreach (XmlNode tableNode in tableNodes)
            {
                string tableName = tableNode.Attributes["name"].Value;
                XmlNodeList fieldNodes = tableNode.SelectNodes(".//field");

                foreach (XmlNode fieldNode in fieldNodes)
                {
                    string fieldName = fieldNode.Attributes["name"].Value;
                    columns.Add($"{tableName}.{fieldName}".ToUpperInvariant());
                }
            }

            return columns;
        }

        public IEnumerable<string> GetTables(string filePath)
        {
            var tables = new List<string>();
            string xml = File.ReadAllText(filePath);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNodeList tableNodes = xmlDoc.SelectNodes("//table");

            foreach (XmlNode tableNode in tableNodes)
            {
                string tableName = tableNode.Attributes["name"].Value;
                tables.Add(tableName.ToUpperInvariant());
            }

            return tables;
        }
    }
}
