using QuikGraph;
using System;

namespace DatabaseMapper.Graph
{
    public class EcalcTableGraph : AdjacencyGraph<EcalcTableGraphVertex, EcalcTableGraphEdge>
    {
        public EcalcTableGraph() : base(allowParallelEdges: false) { }
        public void AddVertex(string tableName) => AddVertex(new EcalcTableGraphVertex(tableName));

    }

    public class EcalcTableGraphEdge : IEdge<EcalcTableGraphVertex>
    {
        public EcalcTableGraphVertex Source { get; }

        public EcalcTableGraphVertex Target { get; }

        public string EdgeLabel { get; }

        public EcalcTableGraphEdge(EcalcTableGraphVertex source, EcalcTableGraphVertex target, string relationLabel)
        {
            Source = source;
            Target = target;
            EdgeLabel = relationLabel;
        }
    }

    public class EcalcTableGraphVertex
    {
        public string Table { get; set; }

        public EcalcTableGraphVertex(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException("Label do vértice é obrigatória");

            Table = tableName;
        }
    }
}
