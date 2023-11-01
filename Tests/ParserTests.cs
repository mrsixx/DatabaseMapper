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
            var relations = parser.ExtractRelationships();
            var tables = parser.ExtractTables();
            Assert.Single(tables);
            Assert.True(tables.ContainsKey("SENHA"));
            Assert.Empty(relations);
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
            var relations = parser.ExtractRelationships();

            Assert.Equal(4, relations.Count);
            Assert.Contains(relations,
                (rel) => rel.Item1 == "EVENTOS.CODSEQ" && rel.Item2 == "TAPONT.CODEVENTO");
            Assert.Contains(relations,
                (rel) => rel.Item1 == "OPERADOR.CODSEQ" && rel.Item2 == "TAPONT.CODOPERADOR");
            Assert.Contains(relations,
                (rel) => rel.Item1 == "MAQUINAS.CODSEQ" && rel.Item2 == "TAPONT.CODMAQUINA");
            Assert.Contains(relations,
                (rel) => rel.Item1 == "PROCS.CODSEQ" && rel.Item2 == "TAPONT.CODPROC");
        }
    }
}
