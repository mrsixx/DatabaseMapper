using QuikGraph;
using System;

namespace DatabaseMapper.Core.Graph
{
    public class TableGraph : AdjacencyGraph<TableGraphVertex, TableGraphEdge>
    {
        public TableGraph() : base(allowParallelEdges: false) { }
        public void AddVertex(string tableName) => AddVertex(new TableGraphVertex(tableName));

    }

    public class TableGraphEdge : IEdge<TableGraphVertex>
    {
        public TableGraphVertex Source { get; }

        public TableGraphVertex Target { get; }

        public string SourceColumn { get; set; }
        public string TargetColumn { get; set; }

        public string SourceLabel => $"{Source.Table}.{SourceColumn.ToUpperInvariant()}";

        public string TargetLabel => $"{Target.Table}.{TargetColumn.ToUpperInvariant()}";

        public string EdgeLabel => $"{SourceLabel} -> {TargetLabel}";

        public TableGraphEdge(TableGraphVertex source, TableGraphVertex target, string sourceColumn, string targetColumn)
        {
            Source = source;
            Target = target;
            SourceColumn = sourceColumn;
            TargetColumn = targetColumn;
        }
    }

    public class TableGraphVertex
    {
        public string Table { get; set; }

        public TableGraphVertex(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException("Label do vértice é obrigatória");

            Table = tableName;
        }
    }
}
