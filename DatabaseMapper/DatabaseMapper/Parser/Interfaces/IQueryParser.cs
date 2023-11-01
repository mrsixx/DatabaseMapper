using System;
using System.Collections.Generic;

namespace DatabaseMapper.Core.Parser.Interfaces
{
    public interface IQueryParser
    {
        Dictionary<string, string> ExtractTables();
        List<Tuple<string, string>> ExtractRelationships();
    }
}
