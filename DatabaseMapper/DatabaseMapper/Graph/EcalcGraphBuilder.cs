using DatabaseMapper.Graph.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseMapper.Graph
{
    public class EcalcGraphBuilder : IEcalcGraphBuilder
    {
        public EcalcColumnGraph BuildColumnsGraph(IEnumerable<string> columns, IEnumerable<Tuple<string, string>> relations)
        {
            var graph = new EcalcColumnGraph();

            foreach (var column in columns)
                graph.AddVertex(column);

            var vertices = graph.Vertices.ToList();
            foreach (var relation in relations)
            {
                var idx1 = vertices.FindIndex(v => v.FullColumnName == relation.Item1);
                var idx2 = vertices.FindIndex(v => v.FullColumnName == relation.Item2);
                if (idx1 != -1 && idx2 != -1)
                {
                    var e1 = vertices[idx1];
                    var e2 = vertices[idx2];
                    var edge = new EcalcColumnGraphEdge(e1, e2);
                    if (!graph.ContainsEdge(e1, e2))
                        graph.AddEdge(edge);
                }
            }

            return graph;
        }

        public EcalcTableGraph BuildTablesGraph(IEnumerable<string> tables, IEnumerable<Tuple<string, string>> relations)
        {
            var graph = new EcalcTableGraph();

            foreach (var table in tables)
                graph.AddVertex(table);

            var vertices = graph.Vertices.ToList();
            foreach (var relation in relations)
            {

                var idx1 = vertices.FindIndex(v => v.Table == relation.Item1.Split('.')[0]);
                var idx2 = vertices.FindIndex(v => v.Table == relation.Item2.Split('.')[0]);
                if (idx1 != -1 && idx2 != -1)
                {
                    var e1 = vertices[idx1];
                    var e2 = vertices[idx2];
                    var edge = new EcalcTableGraphEdge(e1, e2, $"{relation.Item1} -> {relation.Item2}");
                    if (!graph.ContainsEdge(edge))
                        graph.AddEdge(edge);
                }
            }

            return graph;
        }
    }
}
