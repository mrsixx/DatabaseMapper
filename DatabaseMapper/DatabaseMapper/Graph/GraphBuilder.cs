using DatabaseMapper.Graph.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseMapper.Graph
{
    public class GraphBuilder : IGraphBuilder
    {
        public ColumnGraph BuildColumnsGraph(IEnumerable<string> columns, IEnumerable<Tuple<string, string>> relations)
        {
            var graph = new ColumnGraph();

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
                    var edge = new ColumnGraphEdge(e1, e2);
                    if (!graph.ContainsEdge(e1, e2))
                        graph.AddEdge(edge);
                }
            }

            return graph;
        }

        public TableGraph BuildTablesGraph(IEnumerable<string> tables, IEnumerable<Tuple<string, string>> relations)
        {
            var graph = new TableGraph();

            foreach (var table in tables)
                graph.AddVertex(table);

            var vertices = graph.Vertices.ToList();
            foreach (var relation in relations)
            {
                var source = relation.Item1.Split('.');
                var target = relation.Item2.Split('.');
                var idx1 = vertices.FindIndex(v => v.Table == source[0]);
                var idx2 = vertices.FindIndex(v => v.Table == target[0]);
                if (idx1 != -1 && idx2 != -1)
                {
                    var e1 = vertices[idx1];
                    var e2 = vertices[idx2];
                    var edge = new TableGraphEdge(e1, e2, source[1], target[1]);
                    if (!graph.ContainsEdge(edge))
                        graph.AddEdge(edge);
                }
            }

            return graph;
        }
    }
}
