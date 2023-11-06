using QuikGraph;
using System;
using System.Linq;

namespace DatabaseMapper.Core.Graph
{
    [Serializable]
    public class TableGraph : AdjacencyGraph<TableGraphVertex, TableGraphEdge>
    {
        public TableGraph() : base(allowParallelEdges: false) { }
        public void AddVertex(string tableName) => AddVertex(new TableGraphVertex(tableName));

    }

    [Serializable]
    public class TableGraphEdge : IEdge<TableGraphVertex>
    {
        public TableGraphVertex Source { get; }

        public TableGraphVertex Target { get; }

        public string SourceColumn { get; set; }
        public string TargetColumn { get; set; }

        public string SourceLabel => $"{Source.GetLabel()}.{SourceColumn.ToUpperInvariant()}";

        public string TargetLabel => $"{Target.GetLabel()}.{TargetColumn.ToUpperInvariant()}";

        public string EdgeLabel => $"{SourceLabel} -- {TargetLabel}";

        public TableGraphEdge(TableGraphVertex source, TableGraphVertex target, string sourceColumn, string targetColumn)
        {
            Source = source;
            Target = target;
            SourceColumn = sourceColumn;
            TargetColumn = targetColumn;
        }
    }
    
    [Serializable]
    public class TableGraphVertex
    {
        public string Table { get; set; }

        public string GetLabel()
        {
            var split = Table.Split('.');
            if (split.Length > 1)
                return split[1];

            return split[0];
        }
        public string GetSchema()
        {
            var split = Table.Split('.');
            if(split.Length > 1)
                return split[0];

            return String.Empty;
        }

        public TableGraphVertex(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException("Label do vértice é obrigatória");

            Table = tableName;
        }
    }
}
