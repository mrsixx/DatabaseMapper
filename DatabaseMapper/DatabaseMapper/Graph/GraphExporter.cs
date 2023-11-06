using DatabaseMapper.Core.Graph.Interfaces;
using QuikGraph;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;
using QuikGraph.Serialization;
using System;
using System.IO;
using System.Linq;

namespace DatabaseMapper.Core.Graph
{
    public class GraphExporter : IGraphExporter
    {

        public void ExportTableGraphToFile(TableGraph graph, string dir, string filename)
        {
            // cria o diretório se ele não existir
            Directory.CreateDirectory(dir);
            var filePath = Path.Combine(dir, $"{filename}.graph");
            // Use o GraphMLSerializer para salvar o grafo
            using (var stream = File.Create(filePath))
            {
                graph.SerializeToBinary(stream);
            }
        }

        public void ExportTableGraphToGraphviz(TableGraph graph, string dir, string filename)
        {
            var clustering = new ClusteredAdjacencyGraph<TableGraphVertex, TableGraphEdge>(graph);
            byte getRandomByte() => Convert.ToByte(new Random().Next(0, 255));
            foreach (var cluster in graph.Vertices.GroupBy(v => v.GetSchema()))
            {
                var c = clustering.AddCluster();
                c.AddVertexRange(cluster.ToList());
            }

            void exportAlgorithm(GraphvizAlgorithm<TableGraphVertex, TableGraphEdge> graphviz)
            {
                graphviz.GraphFormat.IsCompounded = true;
                graphviz.CommonVertexFormat.Font = new GraphvizFont("Arial", 8);
                graphviz.CommonVertexFormat.FixedSize = false;
                graphviz.CommonVertexFormat.FillColor = GraphvizColor.White;
                graphviz.CommonVertexFormat.FontColor = GraphvizColor.Black;
                graphviz.CommonVertexFormat.Shape = GraphvizVertexShape.Circle;
                graphviz.CommonEdgeFormat.Font = new GraphvizFont("Arial", 3);
                graphviz.CommonEdgeFormat.Label.Angle = 0;
                graphviz.CommonEdgeFormat.TailArrow = new GraphvizArrow(GraphvizArrowShape.Curve);

                graphviz.FormatVertex += (sender, args) =>
                {
                    args.VertexFormat.Label = args.Vertex.GetLabel();
                    args.VertexFormat.Group = args.Vertex.GetSchema();
                };

                graphviz.FormatEdge += (sender, args) =>
                {
                    args.EdgeFormat.Label.Value = args.Edge.EdgeLabel;
                };

                if (clustering.ClustersCount > 1)
                {
                    var i = 0;
                    graphviz.FormatCluster += (sender, args) =>
                    {

                        args.GraphFormat.Label = args.Cluster.Vertices.First().GetSchema();
                        //args.GraphFormat.BackgroundColor = new GraphvizColor(byte.MaxValue, getRandomByte(), getRandomByte(), getRandomByte());
                        i++;
                    };
                }
                var location = Path.Combine(dir, $"{filename}.dot");
                graphviz.Generate(new FileDotEngine(), location);
            }
            clustering.ToGraphviz(exportAlgorithm);
        }

        public TableGraph ImportTableGraphFromFile(string filePath)
        {
            // Use o GraphMLSerializer para salvar o grafo
            using (var stream = new StreamReader(filePath))
            {
                return stream.BaseStream.DeserializeFromBinary<TableGraphVertex, TableGraphEdge, TableGraph>();
            }
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
