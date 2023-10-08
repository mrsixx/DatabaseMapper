using QuikGraph;
using System;

namespace DatabaseMapper.Graph
{
    public class ColumnGraph : AdjacencyGraph<ColumnGraphVertex, ColumnGraphEdge>
    {
        public ColumnGraph() : base(allowParallelEdges: true) { }
        public void AddVertex(string columnName) => AddVertex(new ColumnGraphVertex(columnName));
        public void AddEdge(Tuple<ColumnGraphVertex, ColumnGraphVertex> pair) => AddEdge(new ColumnGraphEdge(pair.Item1, pair.Item2));
    }

    public class ColumnGraphEdge : IEdge<ColumnGraphVertex>
    {
        public ColumnGraphVertex Source { get; }

        public ColumnGraphVertex Target { get; }

        public ColumnGraphEdge(ColumnGraphVertex source, ColumnGraphVertex target)
        {
            Source = source;
            Target = target;
        }
    }

    public class ColumnGraphVertex
    {
        public string Table { get; set; }
        public string Column { get; set; }

        public string FullColumnName => $"{Table}.{Column}";

        public ColumnGraphVertex(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException("Label do vértice é obrigatória");
            var columnNameList = columnName.Split('.');

            if (columnNameList.Length < 2)
                throw new Exception("Nome da coluna deve conter no mínimo tabela e coluna");

            Table = columnNameList[0];
            Column = columnNameList[1];
        }
    }
}
