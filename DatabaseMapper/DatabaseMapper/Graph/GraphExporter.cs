using Antlr4.Runtime.Misc;
using DatabaseMapper.Core.Graph.Interfaces;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

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

                if(clustering.ClustersCount > 1)
                {
                    var i = 0;
                    graphviz.FormatCluster += (sender, args) =>
                    {
                    
                        args.GraphFormat.Label = args.Cluster.Vertices.First().GetSchema();
                        args.GraphFormat.BackgroundColor = new GraphvizColor(byte.MaxValue, getRandomByte(), getRandomByte(), getRandomByte());
                        i++;
                    };
                }
                graphviz.Generate(new FileDotEngine(), location);
            }
            clustering.ToGraphviz(exportAlgorithm);

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
