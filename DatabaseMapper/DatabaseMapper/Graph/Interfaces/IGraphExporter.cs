using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseMapper.Graph.Interfaces
{
    public interface IGraphExporter
    {
        void ExportColumnGraphToGraphviz(EcalcColumnGraph graph, string location);
        void ExportTableGraphToGraphviz(EcalcTableGraph graph, string location);
    }
}
