using DatabaseMapper.Core.Parser;
using System.Linq;
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
            Assert.Contains(metadata.Tables, table => table.TableName == "SENHA");
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
                (rel) => rel.LeftTable == "EVENTOS.CODSEQ" && rel.RightTable == "TAPONT.CODEVENTO");
            Assert.Contains(metadata.Relations,
                (rel) => rel.LeftTable == "OPERADOR.CODSEQ" && rel.RightTable == "TAPONT.CODOPERADOR");
            Assert.Contains(metadata.Relations,
                (rel) => rel.LeftTable == "MAQUINAS.CODSEQ" && rel.RightTable == "TAPONT.CODMAQUINA");
            Assert.Contains(metadata.Relations,
                (rel) => rel.LeftTable == "PROCS.CODSEQ" && rel.RightTable == "TAPONT.CODPROC");
        }




        [Fact]
        public void QueryWithEcalcFilters()
        {
            var query = @"EFILTER &filtroDtDe DATE 'Cdata de' DEFAULT ontem;
                        EFILTER &filtroDtAte DATE AS 'Cdata até' DEFAULT hoje;
                        select first 10 * from arqos
                            where (1=1)
	                        and cdata between &filtroDtDe and &filtroDtAte
                            order by cdata desc";

            var parser = new QueryParser(query);
            var metadata = parser.ExtractMetadata();

            Assert.Equal(2, metadata.EcalcFilters.Count);
            Assert.Contains(metadata.EcalcFilters,
                filter => filter.Name == "&filtroDtDe" && filter.Alias == "Cdata de" && filter.DefaultValue == "ontem");
            Assert.Contains(metadata.EcalcFilters,
                filter => filter.Name == "&filtroDtAte" && filter.Alias == "Cdata até" && filter.DefaultValue == "hoje");

        }

        [Fact]
        public void QueryWithEcalcFiltersRequiredParameters()
        {
            var query = @"EFILTER &filtroDtDe DATE 'Cdata de';
                        EFILTER &filtroDtAte DATE DEFAULT hoje;
                        select first 10 * from arqos
                            where (1=1)
	                        and cdata between &filtroDtDe and &filtroDtAte
                            order by cdata desc";

            var parser = new QueryParser(query);
            var metadata = parser.ExtractMetadata();

            Assert.Equal(2, metadata.EcalcFilters.Count);

            Assert.Equal(1, metadata.EcalcFilters.Count(filter => filter.HasAlias));
            Assert.Equal(1, metadata.EcalcFilters.Count(filter => filter.HasDefaultValue));
            Assert.Contains(metadata.EcalcFilters,
                filter => filter.Name == "&filtroDtDe" && filter.Alias == "Cdata de");
            Assert.Contains(metadata.EcalcFilters,
                filter => filter.Name == "&filtroDtAte" && filter.DefaultValue == "hoje");

        }

        [Fact]
        public void ExtractSqlQueryFromEcalcSqlQuery()
        {
            var ecalcQuery = @"EFILTER &filtroDtDe DATE 'Cdata de';
                        EFILTER &filtroDtAte DATE DEFAULT hoje;
                        select first 10 * from arqos
                            where (1=1)
	                        and cdata between &filtroDtDe and &filtroDtAte
                            order by cdata desc";

            var sqlQuery = @"select first 10 * from arqos
                            where (1=1)
	                        and cdata between &filtroDtDe and &filtroDtAte
                            order by cdata desc";

            var parser = new QueryParser(ecalcQuery);
            var metadata = parser.ExtractMetadata();

            Assert.Contains(sqlQuery, metadata.RealQuery);
            Assert.DoesNotContain("EFILTER", metadata.RealQuery);
        }


    }
}
