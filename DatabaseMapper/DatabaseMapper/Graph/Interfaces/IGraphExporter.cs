using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseMapper.Core.Graph.Interfaces
{
    public interface IGraphExporter
    {
        void ExportColumnGraphToGraphviz(ColumnGraph graph, string location);
        void ExportTableGraphToGraphviz(TableGraph graph, string location);
    }
}
