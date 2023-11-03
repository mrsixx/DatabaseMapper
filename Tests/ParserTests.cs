using DatabaseMapper.Core.Parser;
using Xunit;

namespace Tests
{
    public class ParserTests
    {

        [Fact]
        public void SimpleQuery()
        {
            var parser = new QueryParser("SELECT * FROM SENHA WHERE CODSEQ = 0");
            var metadata = parser.ExtractMetadata();
            Assert.Single(metadata.Tables);
            Assert.True(metadata.Tables.ContainsKey("SENHA"));
            Assert.Empty(metadata.Relations);
        }


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

            var parser = new QueryParser(query);
            var metadata = parser.ExtractMetadata();

            Assert.Equal(4, metadata.Relations.Count);
            Assert.Contains(metadata.Relations,
                (rel) => rel.Item1 == "EVENTOS.CODSEQ" && rel.Item2 == "TAPONT.CODEVENTO");
            Assert.Contains(metadata.Relations,
                (rel) => rel.Item1 == "OPERADOR.CODSEQ" && rel.Item2 == "TAPONT.CODOPERADOR");
            Assert.Contains(metadata.Relations,
                (rel) => rel.Item1 == "MAQUINAS.CODSEQ" && rel.Item2 == "TAPONT.CODMAQUINA");
            Assert.Contains(metadata.Relations,
                (rel) => rel.Item1 == "PROCS.CODSEQ" && rel.Item2 == "TAPONT.CODPROC");
        }
    }
}
