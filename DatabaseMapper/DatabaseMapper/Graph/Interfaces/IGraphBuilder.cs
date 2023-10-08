using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseMapper.Graph.Interfaces
{
    public interface IGraphBuilder
    {
        ColumnGraph BuildColumnsGraph(IEnumerable<string> columns, IEnumerable<Tuple<string, string>> relations);
        TableGraph BuildTablesGraph(IEnumerable<string> tables, IEnumerable<Tuple<string, string>> relations);
    }
}
