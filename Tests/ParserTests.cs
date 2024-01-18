using DatabaseMapper.Core.Parser;
using DatabaseMapper.Core.Parser.Exceptions;
using DatabaseMapper.Core.Parser.Interfaces;
using DatabaseMapper.Core.Parser.Models;
using System;
using System.Linq;
using Xunit;
using static DatabaseMapper.Core.Parser.Enums.DefaultValueFuncionEnum;

namespace Tests
{
    public class ParserTests
    {

        private readonly IQueryParser _queryParser;

        public ParserTests()
        {
            _queryParser = new QueryParser();
        }

        [Fact]
        public void SimpleQuery()
        {
            var metadata = _queryParser.ExtractQueryMetadata("SELECT * FROM SENHA WHERE CODSEQ = 0");
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

            var metadata = _queryParser.ExtractQueryMetadata(query);

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
        public void QueryWithCustomFilters()
        {
            var query = @"EFILTER &filtroDtDe DATE 'Cdata de' DEFAULT ontem;
                        EFILTER &filtroDtAte DATE AS 'Cdata até' DEFAULT hoje;
                        select first 10 * from arqos
                            where (1=1)
	                        and cdata between &filtroDtDe and &filtroDtAte
                            order by cdata desc";

            var metadata = _queryParser.ExtractQueryMetadata(query);

            Assert.Equal(2, metadata.Filters.Count);
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtroDtDe"
                && filter.Alias == "Cdata de"
                && filter.DefaultValue is FilterFunctionValue defaultValue
                && defaultValue.Equals(DefaultValueFunction.Ontem));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtroDtAte"
                && filter.Alias == "Cdata até"
                && filter.DefaultValue is FilterFunctionValue defaultValue
                && defaultValue.Equals(DefaultValueFunction.Hoje));

        }

        [Fact]
        public void QueryWithCustomFilterDetail()
        {
            var query = @"EFILTER &filtro1 INTEGER 'Código x' DEFAULT 20 DETAIL;
                        EFILTER &filtro2 INTEGER AS 'Código pai' DETAIL;
                        select first 10 * from arqos
                            where (1=1)
	                        and codseq between &filtro1 and &filtro2
                            order by cdata desc";

            var metadata = _queryParser.ExtractQueryMetadata(query);

            Assert.Equal(2, metadata.Filters.Count);
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro1"
                && !filter.IsDetail
                && filter.DefaultValue is FilterIntegerValue defaultValue
                && defaultValue.Equals(20));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro2"
                && filter.IsDetail
                && filter.DefaultValue is null);
        }

        [Fact]
        public void QueryWithCustomFiltersRequiredParameters()
        {
            var query = @"EFILTER &filtroDtDe DATE 'Cdata de';
                        EFILTER &filtroDtAte DATE DEFAULT hoje;
                        select first 10 * from arqos
                            where (1=1)
	                        and cdata between &filtroDtDe and &filtroDtAte
                            or cdata > &filtroDtDe
                            order by cdata desc";

            var metadata = _queryParser.ExtractQueryMetadata(query);

            Assert.Equal(2, metadata.Filters.Count);

            Assert.Equal(1, metadata.Filters.Count(filter => filter.HasAlias));
            Assert.Equal(1, metadata.Filters.Count(filter => filter.HasDefaultValue));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtroDtDe" && filter.Alias == "Cdata de");
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtroDtAte"
                && filter.DefaultValue is FilterFunctionValue defaultValue 
                && defaultValue.Equals(DefaultValueFunction.Hoje));

        }

        [Fact]
        public void QueryWithCustomFiltersMismatchedDefaultValues()
        {
            Assert.Throws<InvalidFunctionTypeException>(() => _queryParser.ExtractQueryMetadata("EFILTER &filtro1 DATE DEFAULT diaatual;"));
            Assert.Throws<InvalidFunctionTypeException>(() => _queryParser.ExtractQueryMetadata("EFILTER &filtro2 DATETIME DEFAULT diaatual;"));
            Assert.Throws<InvalidFunctionTypeException>(() => _queryParser.ExtractQueryMetadata("EFILTER &filtro4 TEXT DEFAULT anoatual;"));
            Assert.Throws<InvalidFunctionTypeException>(() => _queryParser.ExtractQueryMetadata("EFILTER &filtro5 DATE DEFAULT usuario;"));
            Assert.Throws<MismatchedTypesException>(() => _queryParser.ExtractQueryMetadata("EFILTER &filtro6 DATE DEFAULT 50.2;"));
            Assert.Throws<MismatchedTypesException>(() => _queryParser.ExtractQueryMetadata("EFILTER &filtro7 DATE DEFAULT 'Hakuna Matata';"));
            Assert.Throws<UnrecognizedTypeException>(() => _queryParser.ExtractQueryMetadata("EFILTER &filtro8 DATE DEFAULT Excelsior"));
        }

        [Fact]
        public void QueryWithUnmapedCustomFilter()
        {
            var query = @"SELECT * FROM USUARIO WHERE ID = &filtroUsuario";
            Assert.Throws<InvalidOperationException>(() => _queryParser.ExtractQueryMetadata(query));
        }


        [Fact]
        public void QueryWithDuplicatedCustomFilterName()
        {
            var query = @"   EFILTER &filtroOntem DATE DEFAULT ontem;
                            EFILTER &filtroOntem DATE DEFAULT hoje;

                        select first 10 * from arqos
                            where (1=1)
	                        and (cdata between &filtroOntem and &filtroOntem)
                            order by cdata desc";
            Assert.Throws<InvalidOperationException>(() => _queryParser.ExtractQueryMetadata(query));
        }

        [Fact]
        public void QueryWithCustomFiltersDefaultValueFunction()
        {
            var query = @"
                        EFILTER &filtro1 INTEGER DEFAULT usuario;
                        EFILTER &filtro2 INTEGER DEFAULT diaatual;
                        EFILTER &filtro3 INTEGER DEFAULT mesatual;
                        EFILTER &filtro4 INTEGER DEFAULT anoatual;
                        EFILTER &filtro5 DATE DEFAULT ontem;
                        EFILTER &filtro6 DATE DEFAULT hoje;
                        EFILTER &filtro7 DATE DEFAULT inicio_mes_atual;
                        EFILTER &filtro8 DATE DEFAULT fim_mes_atual;

                        select first 10 * from arqos
                            where (1=1)
	                        and (cdata between &filtro5 and &filtro6
                            order by cdata desc";

            var metadata = _queryParser.ExtractQueryMetadata(query);


            Assert.Equal(8, metadata.Filters.Count);
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro1"
                          && filter.DefaultValue is FilterFunctionValue functionValue
                          && functionValue.Equals(DefaultValueFunction.Usuario));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro2"
                          && filter.DefaultValue is FilterFunctionValue functionValue
                          && functionValue.Equals(DefaultValueFunction.DiaAtual));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro3"
                          && filter.DefaultValue is FilterFunctionValue functionValue
                          && functionValue.Equals(DefaultValueFunction.MesAtual));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro4"
                          && filter.DefaultValue is FilterFunctionValue functionValue
                          && functionValue.Equals(DefaultValueFunction.AnoAtual));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro5" 
                          && filter.DefaultValue is FilterFunctionValue functionValue
                          && functionValue.Equals(DefaultValueFunction.Ontem));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro6"
                          && filter.DefaultValue is FilterFunctionValue functionValue
                          && functionValue.Equals(DefaultValueFunction.Hoje));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro7"
                          && filter.DefaultValue is FilterFunctionValue functionValue
                          && functionValue.Equals(DefaultValueFunction.InicioMesAtual));
            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro8"
                          && filter.DefaultValue is FilterFunctionValue functionValue
                          && functionValue.Equals(DefaultValueFunction.FimMesAtual));

        }

        [Fact]
        public void QueryWithEcalcFiltersDefaultValueConstant()
        {
            var query = @"
                        EFILTER &filtro1 INTEGER DEFAULT 10;
                        EFILTER &filtro2 DECIMAL DEFAULT 2123.5;
                        EFILTER &filtro3 DATE DEFAULT '2024-04-01';
                        EFILTER &filtro4 DATETIME DEFAULT '2024-04-01 12:35:00';
                        EFILTER &filtro5 TEXT DEFAULT 'Lorem ipsum dolor sit amet';

                        select first 10 * from arqos
                            where (1=1)
	                        and (cdata between &filtro5 and &filtro6
                            or (cdata > &filtro7 and cdata < &filtro8))
                            and (codseq = &filtro1 or codseq != &filtro2 or codseq != &filtro2)
                            order by cdata desc";

            var metadata = _queryParser.ExtractQueryMetadata(query);

            Assert.Equal(5, metadata.Filters.Count);

            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro1"
                          && filter.DefaultValue is FilterIntegerValue defaultValue
                          && defaultValue.Equals(10));

            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro2"
                          && filter.DefaultValue is FilterDecimalValue defaultValue
                          && defaultValue.Equals(2123.5m));

            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro3"
                          && filter.DefaultValue is FilterDateTimeValue defaultValue
                          && defaultValue.Equals(new DateTime(2024, 4, 1)));

            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro4"
                          && filter.DefaultValue is FilterDateTimeValue defaultValue
                          && defaultValue.Equals(new DateTime(2024, 4, 1, 12, 35, 0)));

            Assert.Contains(metadata.Filters,
                filter => filter.Name == "&filtro5"
                          && filter.DefaultValue is FilterTextValue defaultValue
                          && defaultValue.Equals("Lorem ipsum dolor sit amet"));
        }

        [Fact]
        public void ExtractRealSqlQuery()
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

            var metadata = _queryParser.ExtractQueryMetadata(ecalcQuery);

            Assert.Contains(sqlQuery, metadata.RealQuery);
            Assert.DoesNotContain("EFILTER", metadata.RealQuery);
        }

    }
}
