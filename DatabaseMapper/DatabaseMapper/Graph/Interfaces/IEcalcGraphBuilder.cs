using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseMapper.Graph.Interfaces
{
    public interface IEcalcGraphBuilder
    {
        EcalcColumnGraph BuildColumnsGraph(IEnumerable<string> columns, IEnumerable<Tuple<string, string>> relations);
        EcalcTableGraph BuildTablesGraph(IEnumerable<string> tables, IEnumerable<Tuple<string, string>> relations);
    }
}
