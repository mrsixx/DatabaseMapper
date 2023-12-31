
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using DatabaseMapper.Core.Parser.Models;

namespace DatabaseMapper.Core.Parser
{
    public class QueryMetadata
    {
        public string RealQuery { get; set; }

        public List<QueryTable> Tables { get; }

        public List<QueryRelation> Relations { get; }

        public List<EcalcFilter> EcalcFilters { get; }

        public QueryMetadata()
        {
            Tables = new List<QueryTable>();
            Relations = new List<QueryRelation>();
            EcalcFilters = new List<EcalcFilter>();
        }

        public void CopyQuery(string realQuery)
        {

        }
    }
}
