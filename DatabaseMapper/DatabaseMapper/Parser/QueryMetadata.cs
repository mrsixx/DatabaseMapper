
using System.Collections.Generic;
using System;

namespace DatabaseMapper.Core.Parser
{
    public class QueryMetadata
    {
        public Dictionary<string, int> Tables { get; }
        public List<Tuple<string, string>> Relations { get; }

        public QueryMetadata()
        {
            Tables = new Dictionary<string, int>();
            Relations= new List<Tuple<string, string>>();
        }

        public void CopyTables(Dictionary<string, int> tables)
        {
            foreach(var table in tables)
                Tables.Add(table.Key, table.Value);
        }

        public void CopyRelationships(List<Tuple<string, string>> relationships) => Relations.AddRange(relationships);
    }
}
