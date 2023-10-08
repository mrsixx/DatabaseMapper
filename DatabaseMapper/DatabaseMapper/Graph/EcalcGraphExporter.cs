using DatabaseMapper.Graph.Interfaces;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;
using System.IO;

namespace DatabaseMapper.Graph
{
    public class EcalcGraphExporter : IGraphExporter
    {
        public void ExportColumnGraphToGraphviz(EcalcColumnGraph graph, string location)
        {
            var graphviz = new GraphvizAlgorithm<EcalcColumnGraphVertex, EcalcColumnGraphEdge>(graph);
            graphviz.FormatVertex += (sender, args) =>
            {
                args.VertexFormat.Label = args.Vertex.FullColumnName;
                args.VertexFormat.Shape = GraphvizVertexShape.Circle;
            };
            graphviz.Generate(new FileDotEngine(), location);
        }

        public void ExportTableGraphToGraphviz(EcalcTableGraph graph, string location)
        {
            var graphviz = new GraphvizAlgorithm<EcalcTableGraphVertex, EcalcTableGraphEdge>(graph);
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
