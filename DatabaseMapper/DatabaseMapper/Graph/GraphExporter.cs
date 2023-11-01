using DatabaseMapper.Core.Graph.Interfaces;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;
using System.IO;

namespace DatabaseMapper.Core.Graph
{
    public class GraphExporter : IGraphExporter
    {
        public void ExportColumnGraphToGraphviz(ColumnGraph graph, string location)
        {
            var graphviz = new GraphvizAlgorithm<ColumnGraphVertex, ColumnGraphEdge>(graph);
            graphviz.FormatVertex += (sender, args) =>
            {
                args.VertexFormat.Label = args.Vertex.FullColumnName;
                args.VertexFormat.Shape = GraphvizVertexShape.Circle;
            };
            graphviz.Generate(new FileDotEngine(), location);
        }

        public void ExportTableGraphToGraphviz(TableGraph graph, string location)
        {
            var graphviz = new GraphvizAlgorithm<TableGraphVertex, TableGraphEdge>(graph);
            graphviz.FormatVertex += (sender, args) =>
            {
                args.VertexFormat.Label = args.Vertex.Table;
                args.VertexFormat.Shape = GraphvizVertexShape.Circle;
            };
            graphviz.FormatEdge += (sender, args) =>
            {
                args.EdgeFormat.Label.Value = args.Edge.EdgeLabel;
                args.EdgeFormat.Font = new GraphvizFont("Arial", 9);
                args.EdgeFormat.Label.Angle = 0;
                args.EdgeFormat.TailArrow = new GraphvizArrow(GraphvizArrowShape.Curve);
            };

            graphviz.Generate(new FileDotEngine(), location);
        }

    }
    public class FileDotEngine : IDotEngine
    {
        public string Run(GraphvizImageType imageType, string dot, string outputFileName)
        {
            using (StreamWriter writer = new StreamWriter(outputFileName))
                writer.Write(dot);

            return Path.GetFileName(outputFileName);
        }
    }
}
