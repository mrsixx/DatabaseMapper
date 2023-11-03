using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseMapper.Core.Graph.Interfaces
{
    public interface IGraphExporter
    {
        TableGraph ImportTableGraphFromFile(string filePath);
        void ExportTableGraphToFile(TableGraph graph, string dir, string filename);
        void ExportTableGraphToGraphviz(TableGraph graph, string dir, string filename);
    }
}
