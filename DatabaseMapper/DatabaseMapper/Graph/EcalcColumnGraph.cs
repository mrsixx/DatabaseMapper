using QuikGraph;
using System;

namespace DatabaseMapper.Graph
{
    public class EcalcColumnGraph : AdjacencyGraph<EcalcColumnGraphVertex, EcalcColumnGraphEdge>
    {
        public EcalcColumnGraph() : base(allowParallelEdges: true) { }
        public void AddVertex(string columnName) => AddVertex(new EcalcColumnGraphVertex(columnName));
        public void AddEdge(Tuple<EcalcColumnGraphVertex, EcalcColumnGraphVertex> pair) => AddEdge(new EcalcColumnGraphEdge(pair.Item1, pair.Item2));
    }

    public class EcalcColumnGraphEdge : IEdge<EcalcColumnGraphVertex>
    {
        public EcalcColumnGraphVertex Source { get; }

        public EcalcColumnGraphVertex Target { get; }

        public EcalcColumnGraphEdge(EcalcColumnGraphVertex source, EcalcColumnGraphVertex target)
        {
            Source = source;
            Target = target;
        }
    }

    public class EcalcColumnGraphVertex
    {
        public string Table { get; set; }
        public string Column { get; set; }

        public string FullColumnName => $"{Table}.{Column}";

        public EcalcColumnGraphVertex(string columnName)
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
