using DatabaseMapper.Graph;
using DatabaseMapper.Mapper;
using System.Linq;
using Xunit;

namespace Tests
{
    public class MapperTests
    {
        [Fact]
        public void QueryIfMultipleJoins()
        {
            var query = @"SELECT m.maquina AS maquina,
                                   o.nome AS operador,
                                   t.inicio AS inicio,
                                   t.os AS os,
                                   procs.referencia AS processo
                            FROM tapont t
                            JOIN eventos e ON (e.codseq = t.codevento)
                            JOIN operador o ON (o.codseq = t.codoperador)
                            JOIN maquinas m ON (m.codseq = t.codmaquina)
                            JOIN procs ON (procs.codseq = t.codproc)
                            WHERE (1=1)
                              AND e.prod = 0;";

            var graph = new TableGraph();
            var mapper = new DbMapperService();

            Assert.Equal(0, graph.EdgeCount);
            Assert.Equal(0, graph.VertexCount);

            mapper.IncrementModel(graph, query);

            Assert.Equal(4, graph.EdgeCount);
            Assert.Equal(5, graph.VertexCount);

            Assert.Contains(graph.Vertices, v => v.Table == "TAPONT");
            Assert.Contains(graph.Vertices, v => v.Table == "EVENTOS");
            Assert.Contains(graph.Vertices, v => v.Table == "OPERADOR");
            Assert.Contains(graph.Vertices, v => v.Table == "MAQUINAS");
            Assert.Contains(graph.Vertices, v => v.Table == "PROCS");

            Assert.Contains(graph.Edges, e => e.SourceLabel == "EVENTOS.CODSEQ" && e.TargetLabel == "TAPONT.CODEVENTO");
            Assert.Contains(graph.Edges, e => e.SourceLabel == "OPERADOR.CODSEQ" && e.TargetLabel == "TAPONT.CODOPERADOR");
            Assert.Contains(graph.Edges, e => e.SourceLabel == "MAQUINAS.CODSEQ" && e.TargetLabel == "TAPONT.CODMAQUINA");
            Assert.Contains(graph.Edges, e => e.SourceLabel == "PROCS.CODSEQ" && e.TargetLabel == "TAPONT.CODPROC");

            var graphExporter = new GraphExporter();
            graphExporter.ExportTableGraphToGraphviz(graph, @"C:\Users\mathe\Documents\graph.txt");
        }
    }
}
