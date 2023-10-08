using DatabaseMapper.Graph;
using DatabaseMapper.Parser;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class MainTests
    {

        [Fact]
        public void SimpleQuery()
        {
            var parser = new QueryParser();
            var relations = parser.ExtractRelationships("SELECT * FROM SENHA WHERE CODSEQ = 0");
            Assert.Empty(relations);

            var strutReader = new StrutReader();
            var graphBuilder = new EcalcGraphBuilder();
            var graphExporter = new EcalcGraphExporter();
            var columns = strutReader.GetColumns(@"C:\Users\mathe\source\repos\AntlrDiscountPlatform\AntlrDiscountPlatform\SqlRelantionshipMapper\Parser\Strut.xml");

            var graph = graphBuilder.BuildColumnsGraph(columns, relations);
            graphExporter.ExportColumnGraphToGraphviz(graph, @"C:\Users\mathe\source\repos\AntlrDiscountPlatform\AntlrDiscountPlatform\SqlRelantionshipMapper\Parser\graph2.txt");

        }


        [Fact]
        public void QueryIfMultipleJoins()
        {
            //var query = @"SELECT m.maquina AS maquina,
            //                       o.nome AS operador,
            //                       t.inicio AS inicio,
            //                       t.os AS os,
            //                       procs.referencia AS processo
            //                FROM tapont t
            //                JOIN eventos e ON (e.codseq = t.codevento)
            //                JOIN operador o ON (o.codseq = t.codoperador)
            //                JOIN maquinas m ON (m.codseq = t.codmaquina)
            //                JOIN procs ON (procs.codseq = t.codproc)
            //                WHERE (1=1)
            //                  AND e.prod = 0;";
            var queries = @" select    ccodigo as codigo,    cnome as nome,    cendereco  as endereco,    cmunicipio as municipio,    cestado as estado,    ccep as cep,    ctelefone as telefone,    ccontato as contato,    cemail as email,     case prospect        when 'T' then 'Prospect'        else 'Cliente'    end from clientes where (1=1) and inativo = 0;select    
 	m.maquina as maquina,    
	o.nome as operador,    
	t.inicio as inicio,    
	t.os as os,    
	p.referencia as processo 
from tapont t 
join eventos e on (e.codseq = t.codevento) 
join operador o on (o.codseq = t.codoperador) 
join maquinas m on (m.codseq = t.codmaquina) 
join procs p on (p.codseq = t.codproc) 
where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;;; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;;;;select
   CX_BANCO as banco,
   SUM(cvalor) as valor,
   SUM(valorreal) as valor_REAL
from cxbanco
where ccodigo = 2
[ comando:and DATA_PAG between {0} and {1}; texto: Data Recebimento; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: inicio_mes_atual; valorPadraoAdicional: fim_mes_atual]
group by CX_BANCO
; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0;Select first 30    p.cnpo as orcamento,    p.cliente as cliente,    p.referencia as descricao,    p.precofinal as valor,    p.data as dataAbertura from pridat p where p.precofinal > 10000 order by p.data desc;Select    p.cnpo as orcamento,    p.cliente as cliente,    p.precofinal as valor,    p.referencia as descricao,      datediff(day, p.validade, current_timestamp) as diasVencidos from pridat p  where p.precofinal > 20000  and p.statuscod = 7;;  select    os.ccliente as cliente,    count(os.cnpo) as oss,    sum(os.cpfinal) as valor  from arqos os   where (1=1)   group by cliente; select * from Saldo_OS_Faturar; select * from Os_Prazo_Entrega_Vencido;;SELECT
    p.codseq as pedido,
    p.DATAINSERCAO as emissao,
    c.cnome as cliente,
    ip.coditem as codigo,
    ip.descricao as produto,
    pp.quantidade as quantidade,
    (pp.quantidade - ip.faturado) * ip.unit as valor,
    pp.data as entrega
from PEDIDOS p
JOIN itemped ip on (ip.codpedido = p.codseq)
join progped pp on(pp.coditemped = ip.codseq)
join clientes c on (c.ccodigo = p.codcliente)
where (1=1)
and (pp.quantidade - ip.faturado) > 0
and ((p.codstatus in ('AGRUPADO', 'ANDAMENTO', 'CONFIRMADO', 'PENDENTE'))
     or (p.codseq in (select o.pedido
                      from ordvenda o
                      WHERE (O.STATUS = 'PREV' OR o.Status = 'CONF'))))
order by pp.data;SELECT * FROM V_FAT_CLI_DETALHADO where 1 = 1 [and codigocliente = ;texto;String;nome;Detalhe];    select   vendedor as Vendedor,    quantos as OSs,    medval as Media,    totval as Valor,     metaval as Meta,    diftotal as Dif,    percatingidot as Perc,    metaatual as Meta,    difatual as Dif_Meta,    percatingidoa as Perc_Atual,    previsaovenda as Previsao,    quantorc as Orcamentos,    totorcval as Valor_Orcamentos,   totorcval * 0.17 as Conversao_Prevista from meta_mensal_bi4([usuario]); select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0;                select            a.cnpo as OS,            a.cpfinal as Valor,            a.cnumero3 as Comissao        from arqos a        join vendedor v on (v.ccodigo = a.codvend1)        join senha s on (s.codvend = v.ccodigo)        where 1 =1        and s.codseq = [usuario]        and extract(month from a.cdata ) = 9        and extract(year from a.cdata ) = 2021 and a.cgeraos <> - 1; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 3 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;     select        p.cliente as Cliente,        cnpo as Orcamento,        precofinal as Valor    from pridat p    join status st on (st.codseq = p.statuscod)    join vendedor v on (v.ccodigo = p.codvend1)    join senha s on (s.codvend = v.ccodigo)    where 1 = 1    and s.codseq = [usuario] and st.aberto <> 1; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    ccodigo as codigo,    cnome as nome,    cendereco  as endereco,    cmunicipio as municipio,    cestado as estado,    ccep as cep,    ctelefone as telefone,    ccontato as contato,    cemail as email,     case prospect        when 'T' then 'Prospect'        else 'Cliente'    end from clientes where (1=1) and inativo = 0;select * from V_META_MENSAL;Select first 30    p.cnpo as orcamento,    p.cliente as cliente,    p.referencia as descricao,    p.precofinal as valor,    p.data as dataAbertura from pridat p where p.precofinal > 10000 order by p.data desc;Select    p.cnpo as orcamento,    p.cliente as cliente,    p.precofinal as valor,    p.referencia as descricao,      datediff(day, p.validade, current_timestamp) as diasVencidos from pridat p  where p.precofinal > 20000  and p.statuscod = 7;  select    os.ccliente as cliente,    count(os.cnpo) as oss,    sum(os.cpfinal) as valor  from arqos os   where (1=1)   group by cliente; select * from Saldo_OS_Faturar; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select * from Os_Prazo_Entrega_Vencido; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;Select first 30    p.cnpo as orcamento,    p.cliente as cliente,    p.referencia as descricao,    p.precofinal as valor,    p.data as dataAbertura from pridat p where p.precofinal > 10000 order by p.data desc;Select    p.cnpo as orcamento,    p.cliente as cliente,    p.precofinal as valor,    p.referencia as descricao,      datediff(day, p.validade, current_timestamp) as diasVencidos from pridat p  where p.precofinal > 20000  and p.statuscod = 7;  select    os.ccliente as cliente,    count(os.cnpo) as oss,    sum(os.cpfinal) as valor  from arqos os   where (1=1)   group by cliente; select * from Os_Prazo_Entrega_Vencido; select * from Saldo_OS_Faturar;select * from Itens_Estoque_Abaixo;select * from compras_aberto; select    ccodigo as codigo,    cnome as nome,    cendereco  as endereco,    cmunicipio as municipio,    cestado as estado,    ccep as cep,    ctelefone as telefone,    ccontato as contato,    cemail as email,     case prospect        when 'T' then 'Prospect'        else 'Cliente'    end from clientes where (1=1) and inativo = 0;select * from V_META_MENSAL; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;SELECT * FROM V_TITULOS_RECEBER_30DIAS;SELECT * FROM V_TITULOS_APAGAR_ATRASO_5DIAS;SELECT * FROM V_TITULOS_RECEBER_ATRASO_10DIAS;SELECT * FROM V_FAT_CLI_MES_ATUAL;Select first 30    p.cnpo as orcamento,    p.cliente as cliente,    p.referencia as descricao,    p.precofinal as valor,    p.data as dataAbertura from pridat p where p.precofinal > 10000 order by p.data desc;Select    p.cnpo as orcamento,    p.cliente as cliente,    p.precofinal as valor,    p.referencia as descricao,      datediff(day, p.validade, current_timestamp) as diasVencidos from pridat p  where p.precofinal > 20000  and p.statuscod = 7;  select    os.ccliente as cliente,    count(os.cnpo) as oss,    sum(os.cpfinal) as valor  from arqos os   where (1=1)   group by cliente; select * from Os_Prazo_Entrega_Vencido; select * from Saldo_OS_Faturar;select * from Itens_Estoque_Abaixo;select * from compras_aberto; select    ccodigo as codigo,    cnome as nome,    cendereco  as endereco,    cmunicipio as municipio,    cestado as estado,    ccep as cep,    ctelefone as telefone,    ccontato as contato,    cemail as email,     case prospect        when 'T' then 'Prospect'        else 'Cliente'    end from clientes where (1=1) and inativo = 0;select * from V_META_MENSAL; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;SELECT * FROM V_TITULOS_RECEBER_30DIAS;SELECT * FROM V_TITULOS_APAGAR_ATRASO_5DIAS;SELECT * FROM V_TITULOS_RECEBER_ATRASO_10DIAS;SELECT * FROM V_FAT_CLI_MES_ATUAL;Select first 30    p.cnpo as orcamento,    p.cliente as cliente,    p.referencia as descricao,    p.precofinal as valor,    p.data as dataAbertura from pridat p where p.precofinal > 10000 order by p.data desc;Select    p.cnpo as orcamento,    p.cliente as cliente,    p.precofinal as valor,    p.referencia as descricao,      datediff(day, p.validade, current_timestamp) as diasVencidos from pridat p  where p.precofinal > 20000  and p.statuscod = 7;  select    os.ccliente as cliente,    count(os.cnpo) as oss,    sum(os.cpfinal) as valor  from arqos os   where (1=1)   group by cliente; select * from Os_Prazo_Entrega_Vencido; select * from Saldo_OS_Faturar;select * from Itens_Estoque_Abaixo;select * from compras_aberto; select    ccodigo as codigo,    cnome as nome,    cendereco  as endereco,    cmunicipio as municipio,    cestado as estado,    ccep as cep,    ctelefone as telefone,    ccontato as contato,    cemail as email,     case prospect        when 'T' then 'Prospect'        else 'Cliente'    end from clientes where (1=1) and inativo = 0;select * from V_META_MENSAL; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;SELECT * FROM V_TITULOS_RECEBER_30DIAS;SELECT * FROM V_TITULOS_APAGAR_ATRASO_5DIAS;SELECT * FROM V_TITULOS_RECEBER_ATRASO_10DIAS;SELECT * FROM V_FAT_CLI_MES_ATUAL;Select first 30    p.cnpo as orcamento,    p.cliente as cliente,    p.referencia as descricao,    p.precofinal as valor,    p.data as dataAbertura from pridat p where p.precofinal > 10000 order by p.data desc;Select    p.cnpo as orcamento,    p.cliente as cliente,    p.precofinal as valor,    p.referencia as descricao,      datediff(day, p.validade, current_timestamp) as diasVencidos from pridat p  where p.precofinal > 20000  and p.statuscod = 7;  select    os.ccliente as cliente,    count(os.cnpo) as oss,    sum(os.cpfinal) as valor  from arqos os   where (1=1)   group by cliente; select * from Os_Prazo_Entrega_Vencido; select * from Saldo_OS_Faturar;select * from Itens_Estoque_Abaixo;select * from compras_aberto; select    ccodigo as codigo,    cnome as nome,    cendereco  as endereco,    cmunicipio as municipio,    cestado as estado,    ccep as cep,    ctelefone as telefone,    ccontato as contato,    cemail as email,     case prospect        when 'T' then 'Prospect'        else 'Cliente'    end from clientes where (1=1) and inativo = 0;select * from V_META_MENSAL; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;SELECT * FROM V_TITULOS_RECEBER_30DIAS;SELECT * FROM V_TITULOS_APAGAR_ATRASO_5DIAS;SELECT * FROM V_TITULOS_RECEBER_ATRASO_10DIAS;SELECT * FROM V_FAT_CLI_MES_ATUAL;Select first 30    p.cnpo as orcamento,    p.cliente as cliente,    p.referencia as descricao,    p.precofinal as valor,    p.data as dataAbertura from pridat p where p.precofinal > 10000 order by p.data desc;Select    p.cnpo as orcamento,    p.cliente as cliente,    p.precofinal as valor,    p.referencia as descricao,      datediff(day, p.validade, current_timestamp) as diasVencidos from pridat p  where p.precofinal > 20000  and p.statuscod = 7;  select    os.ccliente as cliente,    count(os.cnpo) as oss,    sum(os.cpfinal) as valor  from arqos os   where (1=1)   group by cliente; select * from Os_Prazo_Entrega_Vencido; select * from Saldo_OS_Faturar;select * from Itens_Estoque_Abaixo;select * from compras_aberto; select    ccodigo as codigo,    cnome as nome,    cendereco  as endereco,    cmunicipio as municipio,    cestado as estado,    ccep as cep,    ctelefone as telefone,    ccontato as contato,    cemail as email,     case prospect        when 'T' then 'Prospect'        else 'Cliente'    end from clientes where (1=1) and inativo = 0;select * from V_META_MENSAL; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;SELECT * FROM V_TITULOS_RECEBER_30DIAS;SELECT * FROM V_TITULOS_APAGAR_ATRASO_5DIAS;SELECT * FROM V_TITULOS_RECEBER_ATRASO_10DIAS;SELECT * FROM V_FAT_CLI_MES_ATUAL;select
    maquina,
    sum(tempoProducao) as tempoProducao,
    sum(custoProducao) as custoProducao,
    sum(totalRotacao) as totalRotacao,
    sum(giros) as giros,
    turno
from (SELECT
              ODTIMP.NOME as maquina,
              (ODTIMP.TEMPOPROD) as tempoProducao,
              (ODTIMP.CUSTOTOTAL) as custoProducao,
              (ODTIMP.PRODMEDIA) as totalRotacao,
              (r.totprod) as giros ,
              (select distinct t.descricao from turnos t where  (( cast(r.inicio as time) between t.inicio and t.fim ) and ( cast(r.fim as time) between t.inicio and t.fim )) ) as turno
            FROM ODTIMP
            JOIN ARQOS ON (ODTIMP.CNPO = ARQOS.CNPO)
            join regop r on (r.os = arqos.cnpo)
            LEFT JOIN STATUS ON (ARQOS.STATUSCOD = STATUS.CODSEQ)
            WHERE (1=1)
            [ comando:and ARQOS.CDATA between {0} and {1}; texto: Abertura da OS; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje]
            and ODTIMP.TEMPOPROD > 0) t
            group by maquina, turno;select
    sum (num_orc_total),
    sum(num_orc_convertidos)
from (
        SELECT
            count(PRI.cnpo) as num_orc_total,
            0 as num_orc_convertidos
        from pridat pri
        where 1=1
       [ comando:and pri.data between {0} and {1}; texto: Orçamento; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje]
               
        union all
        
        SELECT
            0 as num_orc_total,
            count(convertidos.cnpo) as num_orc_convertidos
        from  pridat convertidos
        --join arqos os on (os.orcagera = convertidos.cnpo)
        join status on (convertidos.statuscod=status.codseq and status.descricao = 'Orçamento Convertido')
        where 1=1
       [ comando: and convertidos.data between {0} and {1}; texto: Conversão; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje]
       ) t;select
    oc.codigo as compra,
    oc.codforn as cod_fornecedor,
    oc.fornec as fornecedor,
    oc.codnatoper as cod_nat_oper,
    oc.nomenatop as nat_oper,
    oc.chegada as previsao_chegada,
    oc.vtotal as valor_total ,
    oc.status,
    oc.saida as emissao,
    s.ident as usuario,
    oc.conta as conta_a_pagar,
    oc.formapagamento,
    oc.toticmsmoney as total_icms,
    oc.totipimoney as total_IPI,
    oc.obs as observacao,
    oc.dest as conta_destino
from ORDCOMPR oc
left join senha s on (s.codseq = oc.usuarioinsert)
where (1=1)
and oc.status <> 'CANCEL'
[comando:and chegada between current_date and dateadd( {0} day to current_date); texto: Próximos dias; tipo: Integer; nome: dias;valorPadrao:15 ];Select first 30    p.cnpo as orcamento,    p.cliente as cliente,    p.referencia as descricao,    p.precofinal as valor,    p.data as dataAbertura from pridat p where p.precofinal > 10000 order by p.data desc;Select    p.cnpo as orcamento,    p.cliente as cliente,    p.precofinal as valor,    p.referencia as descricao,      datediff(day, p.validade, current_timestamp) as diasVencidos from pridat p  where p.precofinal > 20000  and p.statuscod = 7;  select    os.ccliente as cliente,    count(os.cnpo) as oss,    sum(os.cpfinal) as valor  from arqos os   where (1=1)   group by cliente; select * from Os_Prazo_Entrega_Vencido;; select * from Saldo_OS_Faturar;select * from Itens_Estoque_Abaixo;select * from compras_aberto; select    ccodigo as codigo,    cnome as nome,    cendereco  as endereco,    cmunicipio as municipio,    cestado as estado,    ccep as cep,    ctelefone as telefone,    ccontato as contato,    cemail as email,     case prospect        when 'T' then 'Prospect'        else 'Cliente'    end from clientes where (1=1) and inativo = 0;select * from V_META_MENSAL; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;SELECT * FROM V_TITULOS_RECEBER_30DIAS;SELECT * FROM V_TITULOS_APAGAR_ATRASO_5DIAS;SELECT * FROM V_TITULOS_RECEBER_ATRASO_10DIAS;SELECT * FROM V_FAT_CLI_MES_ATUAL;Select first 30    p.cnpo as orcamento,    p.cliente as cliente,    p.referencia as descricao,    p.precofinal as valor,    p.data as dataAbertura from pridat p where p.precofinal > 10000 order by p.data desc;Select    p.cnpo as orcamento,    p.cliente as cliente,    p.precofinal as valor,    p.referencia as descricao,      datediff(day, p.validade, current_timestamp) as diasVencidos from pridat p  where p.precofinal > 20000  and p.statuscod = 7;  select    os.ccliente as cliente,    count(os.cnpo) as oss,    sum(os.cpfinal) as valor  from arqos os   where (1=1)   group by cliente; select * from Os_Prazo_Entrega_Vencido;; select * from Saldo_OS_Faturar;select * from Itens_Estoque_Abaixo;select * from compras_aberto; select    ccodigo as codigo,    cnome as nome,    cendereco  as endereco,    cmunicipio as municipio,    cestado as estado,    ccep as cep,    ctelefone as telefone,    ccontato as contato,    cemail as email,     case prospect        when 'T' then 'Prospect'        else 'Cliente'    end from clientes where (1=1) and inativo = 0;select * from V_META_MENSAL; select    m.maquina as maquina,    o.nome as operador,    t.inicio as inicio,    t.os as os,    p.referencia as processo from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0; select    m.maquina as maquina,    o.nome as operador,    p.inicio as inicio,    p.termino as termino,    t.os as os,    p.referencia as processo,    p.tempoprod as tempoReal,    p.tempoprodoriginal as tempoPrevisto,    (p.tempoprodoriginal - p.tempoprod) as tempoExcedido from tapont t join eventos e on (e.codseq = t.codevento) join operador o on (o.codseq = t.codoperador) join maquinas m on (m.codseq = t.codmaquina) join procs p on (p.codseq = t.codproc) where (1=1) and e.prod = 1 and (p.tempoprodoriginal - p.tempoprod) > 0;SELECT * FROM V_TITULOS_RECEBER_30DIAS;SELECT * FROM V_TITULOS_APAGAR_ATRASO_5DIAS;SELECT * FROM V_TITULOS_RECEBER_ATRASO_10DIAS;SELECT * FROM V_FAT_CLI_MES_ATUAL;select
    ccodigo as codigo,
    cnome as item,
    cestatual  as estoqueAtual,
    cestminimo as cestoqueMinimo
from itens
where (1=1)
and (cestatual - cestminimo) < 0;;select * from compras_aberto;;;;;;;select distinct
    cast(os.cnpo as int) as os,
    os.ccliente as cliente,
    os.cref as titulo,
    os.cquant as tiragem,
    os.cdata as dataAbertura,
    os.prazo as prazoCliente,
    atb.respdata as dataAprovacao,
    v.cnome as vendedor,
    coalesce(odt.nomeproc, odtimp.processoimp) as status,  --validar com cliente
    (select nomeproc from odtacab where odtacab.cnpo = os.cnpo and (odtacab.tipo = 2 or odtacab.TIPOCALC =2) ) as acabamento, --validar com cliente
    --m.maquina as maquina,
    coalesce(imp.nome, m.maquina) as maquina,
    os.observacoes as observacoes --validar com cliente
from arqos os
join procs p on (p.os = os.cnpo)  --apenas programados na grade
left join maquinas m on (m.codseq = p.maquina)
left join maqimp imp on (imp.codseq = p.codseqimp)
join vendedor v on (os.codvend1 = v.ccodigo)
left join cliatrb atb on (atb.codcliente = cast(cast(os.cnpo as int) || '/0' as varchar(30)) and atb.atributo = 31 and atb.origem = 'OS')
left join tapont apAcab on (apAcab.os = os.cnpo and apAcab.plano = p.plano and apAcab.tipoproc = 1)
left join odtacab odt on (odt.codseq = apAcab.codproc )
left join tapont apImp on (apImp.os = os.cnpo and apImp.plano = p.plano and apImp.tipoproc = 2)
left join odtimp odtimp on (odtimp.codseq = apAcab.codproc )
where (1=1)
and os.cgeraos not in (-1,4);select
    os.ccliente as cliente,
    os.cnpo as os,
    os.cref as titulo,
    osf.preco as valor,
    osf.quant as quantidade,
    osf.datafat entrega
from osfat osf
join arqos os on (os.cnpo = osf.os)
where (1=1)
[ comando: and EXTRACT(month FROM osf.datafat) = {0}; texto: Mês; tipo: Integer; nome: mes;valorPadrao: mesatual]
[ comando: and EXTRACT(YEAR FROM osf.datafat) = {0}; texto: Ano; tipo: Integer; nome: ano; valorPadrao: anoatual];select
    os.ccliente as cliente,
    os.cnpo as os,
    os.cref as titulo,
    os.cpfinal as valor,
    os.cquant as quantidade,
    os.dataentrega as entrega
from arqos  os
where (1=1)
and os.cgeraos <> -1
and not exists (select os from osfat osf where osf.os = os.cnpo)
[ comando: and EXTRACT(month FROM os.dataentrega) = {0}; texto: Mês; tipo: Integer; nome: mes; valorPadrao: mesatual ]
[ comando: and EXTRACT(YEAR FROM os.dataentrega) = {0}; texto: Ano; tipo: Integer; nome: ano; valorPadrao: anoatual ];select
    os.ccliente as cliente,
    os.cnpo as os,
    os.cref as titulo,
    os.cpfinal as valor,
    os.cquant as quantidade,
    os.dataentrega as entrega
from arqos  os
where (1=1)
and os.cgeraos <> -1
[ comando: and EXTRACT(month FROM os.cdata) = {0}; texto: Mês; tipo: Integer; nome: mes; valorPadrao: mesatual ]
[ comando: and EXTRACT(YEAR FROM os.cdata) = {0}; texto: Ano; tipo: Integer; nome: ano; valorPadrao: anoatual ];select distinct
    cast(os.cnpo as int) as os,
    os.ccliente as cliente,
    os.cref as titulo,
    os.cquant as tiragem,
    os.cdata as dataAbertura,
    os.prazo as prazoCliente,
    coalesce(atb.respdata, '') as dataAprovacao,
    v.cnome as vendedor,    
    coalesce(acabamento.resptext, '') as acabamento,
    coalesce(imp.nome, '') as maquina,
    os.observacoes as observacoes, --validar com cliente
    coalesce(rProva.resposta, '') as prova,
    coalesce(rPadraoCor.resposta,'') as padraoCor,
    coalesce(rCompraExcedente.resposta, '') as aceitaExcedente,
    coalesce(rEnvioModelo.resposta, '') as envioModelo,
    coalesce(rEspecEmbalagem.resposta, '') as especificacaoEmbalagem,
    coalesce(rCaixaFornecida.resposta, '') as caixaFornecida,
    p.referencia as status 
from procs p
join arqos os on (os.cnpo = p.os )
join vendedor v on (os.codvend1 = v.ccodigo)
join maqimp imp on (imp.codseq = p.codseqimp)
left join cliatrb atb on (atb.codcliente = cast(cast(os.cnpo as int) || '/0' as varchar(30)) and atb.atributo = 31 and atb.origem = 'OS')
left join cliatrb prova on (prova.codcliente = cast(cast(os.cnpo as int) || '/0' as varchar(30)) and prova.atributo = 32 and prova.origem = 'OS')
left join respatrb rProva on (rProva.codseq = prova.resposta)
left join cliatrb padraoCor on (padraoCor.codcliente = cast(cast(os.cnpo as int) || '/0' as varchar(30)) and padraoCor.atributo = 33 and padraoCor.origem = 'OS')
left join respatrb rPadraoCor on (rPadraoCor.codseq = padraoCor.resposta)
left join cliatrb compraExcedente on (compraExcedente.codcliente = cast(cast(os.cnpo as int) || '/0' as varchar(30)) and compraExcedente.atributo = 34 and compraExcedente.origem = 'OS')
left join respatrb rCompraExcedente on (rCompraExcedente.codseq = compraExcedente.resposta)
left join cliatrb envioModelo on (envioModelo.codcliente = cast(cast(os.cnpo as int) || '/0' as varchar(30)) and envioModelo.atributo = 35 and envioModelo.origem = 'OS')
left join respatrb rEnvioModelo on (rEnvioModelo.codseq = envioModelo.resposta)
left join cliatrb especEmbalagem on (especEmbalagem.codcliente = cast(cast(os.cnpo as int) || '/0' as varchar(30)) and especEmbalagem.atributo = 36 and especEmbalagem.origem = 'OS')
left join respatrb rEspecEmbalagem on (rEspecEmbalagem.codseq = especEmbalagem.resposta)
left join cliatrb caixaFornecida on (caixaFornecida.codcliente = cast(cast(os.cnpo as int) || '/0' as varchar(30)) and caixaFornecida.atributo = 37 and caixaFornecida.origem = 'OS')
left join respatrb rCaixaFornecida on (rCaixaFornecida.codseq = caixaFornecida.resposta)
left join cliatrb acabamento on (acabamento.codcliente = cast(cast(os.cnpo as int) || '/0' as varchar(30)) and acabamento.atributo = 38 and acabamento.origem = 'OS')
left join tapont ap on (ap.os = os.cnpo)
left join grupos gp on (gp.codseq = ap.grupopcp)
left join eventos e on (e.codseq = ap.codevento)
--left join tapont apAcab on (apAcab.os = os.cnpo and apAcab.plano = p.plano and apAcab.tipoproc = 1)
--left join odtacab odt on (odt.codseq = apAcab.codproc )
--left join tapont apImp on (apImp.os = os.cnpo  and apImp.tipoproc = 2)
--left join odtimp odtimp on (odtimp.codseq = apAcab.codproc )
where (1=1)
and os.cgeraos not in (-1,4)
and os.posicaoos <> 2;with cx as(
    select distinct sum(cx.valorreal) over (partition by (extract (month from cx.data_pag) || extract (year from cx.data_pag))) valor,
    (EXTRACT (MONTH FROM cx.data_pag)  ) as mes,
    (EXTRACT (year FROM cx.data_pag)  ) as ano
    from cxbanco cx
    join entrega e on (e.venda = cx.VINCULON)
    JOIN CLIENTES C ON (C.CCODIGO = E.CODCLIENTE)
    where (1=1)
    and cx.ccodigo=2
    
    )

select
    ano,
    sum(iif(mes = 1, valor, 0)) as janeiro,
    sum(iif(mes = 2, valor, 0)) as fevereiro,
    sum(iif(mes = 3, valor, 0)) as marco,
    sum(iif(mes = 4, valor, 0)) as abril,
    sum(iif(mes = 5, valor, 0)) as maio,
    sum(iif(mes = 6, valor, 0)) as junho,
    sum(iif(mes = 7, valor, 0)) as julho,
    sum(iif(mes = 8, valor, 0)) as agosto,
    sum(iif(mes = 9, valor, 0)) as setembro,
    sum(iif(mes = 10, valor, 0)) as outubro,
    sum(iif(mes = 11, valor, 0)) as novembro,
    sum(iif(mes = 12, valor, 0)) as dezembro
 from cx
 group by ano;;with cx as(
    SELECT distinct
        EXTRACT (MONTH FROM E.SAIDA)  as mes,
        EXTRACT (YEAR FROM E.SAIDA) AS ano,
        SUM(I.PTOTAL) over (partition by (extract (month from e.saida) || extract (year from e.saida))) valor
    FROM
      ENTREGA E
      JOIN ITEMENTR I ON (E.CODIGO = I.CODIGOENTREGA)
      JOIN CLIENTES C ON (C.CCODIGO = E.CODCLIENTE)
      JOIN NATOPER NT ON (NT.CODIGO = I.CODAUTONATOPER)
    WHERE
    (E.STATUS <> 'CANC')
    AND NT.EMITETIT <> 'T'
    AND (E.TIPO = 0)
        
    UNION ALL
    
    SELECT distinct
        EXTRACT (MONTH FROM E.SAIDA)  as mes,
        EXTRACT (YEAR FROM E.SAIDA) AS ano,
        SUM(I.PTOTAL) over (partition by (extract (month from e.saida) || extract (year from e.saida))) valor
    FROM
     ENTREGA E
      JOIN PRODENTR I ON (E.CODIGO = I.CODIGOENTREGA)
      JOIN CLIENTES C ON (C.CCODIGO = E.CODCLIENTE)
      JOIN NATOPER NT ON (NT.CODIGO = I.CODAUTONATOPER)
    WHERE
    (E.STATUS <> 'CANC')
    AND NT.EMITETIT <> 'T'
    AND (E.TIPO = 0)
    
    union all

     SELECT distinct
        EXTRACT (MONTH FROM E.SAIDA)  as mes,
        EXTRACT (YEAR FROM E.SAIDA) AS ano,
        SUM(I.PTOTAL) over (partition by (extract (month from e.saida) || extract (year from e.saida))) valor
    FROM
     ENTREGA E
      JOIN serventr I ON (E.CODIGO = I.CODIGOENTREGA)
      JOIN CLIENTES C ON (C.CCODIGO = E.CODCLIENTE)
      JOIN NATOPER NT ON (NT.CODIGO = I.CODAUTONATOPER)
    WHERE
    (E.STATUS <> 'CANC')
    AND (E.TIPO = 0)
    
    union all

    SELECT  distinct
        EXTRACT (MONTH FROM pp.data)  as mes,
        EXTRACT (YEAR FROM pp.data) AS ano,
        SUM((pp.quantidade - ip.entregue) * ip.unit) over (partition by (extract (month from pp.data) || extract (year from pp.data))) valor

from PEDIDOS p
JOIN itemped ip on (ip.codpedido = p.codseq)
join progped pp on(pp.coditemped = ip.codseq)
join clientes c on (c.ccodigo = p.codcliente)
where (1=1)
and (pp.quantidade - ip.entregue) > 0
and ((p.codstatus in ('AGRUPADO', 'ANDAMENTO', 'CONFIRMADO', 'PENDENTE'))
     or (p.codseq in (select o.pedido
                      from ordvenda o
                      WHERE (O.STATUS = 'PREV' OR o.Status = 'CONF'))))
    )

select
    ano,
    sum(iif(mes = 1, valor, 0)) as janeiro,
    sum(iif(mes = 2, valor, 0)) as fevereiro,
    sum(iif(mes = 3, valor, 0)) as marco,
    sum(iif(mes = 4, valor, 0)) as abril,
    sum(iif(mes = 5, valor, 0)) as maio,
    sum(iif(mes = 6, valor, 0)) as junho,
    sum(iif(mes = 7, valor, 0)) as julho,
    sum(iif(mes = 8, valor, 0)) as agosto,
    sum(iif(mes = 9, valor, 0)) as setembro,
    sum(iif(mes = 10, valor, 0)) as outubro,
    sum(iif(mes = 11, valor, 0)) as novembro,
    sum(iif(mes = 12, valor, 0)) as dezembro
 from cx
 group by ano


    


;select distinct
A.CLIENTE,
SUM(A.PRECO_TOTAL_ITEM) AS TOTAL,
dense_rank() over( order by SUM(A.PRECO_TOTAL_ITEM) desc) as ranking
from (
		select * from 
			(SELECT  MF.MES AS MES , MF.ANO AS ANO, E.entrega,
				(I.PTOTAL) AS PRECO_TOTAL_ITEM,
				 c.cnome AS CLIENTE
			FROM ENTREGA E
			  JOIN v_mes_faturamento MF ON (MF.entrega = E.codigo)
			  JOIN ITEMENTR I ON (E.CODIGO = I.CODIGOENTREGA)
			  JOIN CLIENTES C ON (C.CCODIGO = E.CODCLIENTE)
			  JOIN VENDEDOR V ON (V.CCODIGO = C.CVENDEDOR1)
				JOIN SENHA S ON (S.CODVEND = V.CCODIGO)
			  JOIN ITENS IT ON (IT.CCODIGO = I.CODITEM)
			  JOIN NATOPER NT ON (NT.CODIGO = I.CODAUTONATOPER)
			WHERE
			(E.STATUS <> 'CANC')
			AND NT.EMITETIT <> 'T'
			AND (E.TIPO = 0)
			 
			UNION ALL
			
			SELECT
				MF.MES AS MES , MF.ANO AS ANO, E.entrega,
				(I.PTOTAL) AS PRECO_TOTAL_ITEM,
				 c.cnome AS CLIENTE
			  FROM ENTREGA E
			  JOIN v_mes_faturamento MF ON (MF.entrega = E.codigo)  
			  JOIN PRODENTR I ON (E.CODIGO = I.CODIGOENTREGA)
			  JOIN CLIENTES C ON (C.CCODIGO = E.CODCLIENTE)
			  JOIN ITENS IT ON (IT.CCODIGO = I.CODITEM)
			  JOIN VENDEDOR V ON (V.CCODIGO = C.CVENDEDOR1)
			  JOIN NATOPER NT ON (NT.CODIGO = I.CODAUTONATOPER)
			WHERE
			(E.STATUS <> 'CANC')
			AND NT.EMITETIT <> 'T'
			AND (E.TIPO = 0))
			WHERE (1=1)
            [ comando:AND ENTREGA BETWEEN {0} AND {1}; texto: Faturamento; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje ]
	) A 
 GROUP BY 1
 order by 3;select distinct
A.MES || '/' ||A.ANO AS DATA_ENTREGA,
SUM(A.PRECO_TOTAL_ITEM) AS TOTAL,
A.CLIENTE,
dense_rank() over(partition by A.mes || '/' || A.ANO order by SUM(A.PRECO_TOTAL_ITEM) desc) as ranking
from (
       select * from
        ( SELECT  MF.MES AS MES , MF.ANO AS ANO, E.entrega as entrega,
            (I.PTOTAL) AS PRECO_TOTAL_ITEM,
             c.cnome AS CLIENTE
        FROM ENTREGA E
          JOIN v_mes_faturamento MF ON (MF.entrega = E.codigo)
          JOIN ITEMENTR I ON (E.CODIGO = I.CODIGOENTREGA)
          JOIN CLIENTES C ON (C.CCODIGO = E.CODCLIENTE)
          JOIN VENDEDOR V ON (V.CCODIGO = C.CVENDEDOR1)
          JOIN ITENS IT ON (IT.CCODIGO = I.CODITEM)
          JOIN NATOPER NT ON (NT.CODIGO = I.CODAUTONATOPER)
        WHERE
        (E.STATUS <> 'CANC')
        AND NT.EMITETIT <> 'T'
        AND (E.TIPO = 0)
    
        
        UNION ALL
        
        SELECT
            MF.MES AS MES , MF.ANO AS ANO, E.entrega,
            (I.PTOTAL) AS PRECO_TOTAL_ITEM,
            c.cnome AS CLIENTE
          FROM ENTREGA E
          JOIN v_mes_faturamento MF ON (MF.entrega = E.codigo)  
          JOIN PRODENTR I ON (E.CODIGO = I.CODIGOENTREGA)
          JOIN CLIENTES C ON (C.CCODIGO = E.CODCLIENTE)
          JOIN ITENS IT ON (IT.CCODIGO = I.CODITEM)
          JOIN VENDEDOR V ON (V.CCODIGO = C.CVENDEDOR1)
          JOIN NATOPER NT ON (NT.CODIGO = I.CODAUTONATOPER)
        WHERE
        (E.STATUS <> 'CANC')
        AND NT.EMITETIT <> 'T'
        AND (E.TIPO = 0) ) 
  where (1=1)
  [ comando:AND ENTREGA BETWEEN {0} AND {1}; texto: Faturamento; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje ]
   ) A
 GROUP BY 1,3
 order by 1, 4;with COMPRA as (
      SELECT
          CAST(SUM(I.QUANTIDADE) AS INT) AS QUANTIDADE_ITEM ,
          EXTRACT (MONTH FROM C.CHEGADA) AS MES,
          I.CODITEM AS CODIGO_ITEM,
          I.NOMEITEM AS NOME_ITEM
      FROM CHEGADA C
      JOIN ITEMCHEG I ON  (I.CODCHEGADA = C.CODIGO)
      WHERE (1=1)
      AND I.DEVOLUCAO <> 'T'
      AND C.CHEGADA BETWEEN DATEADD (MONTH, -12, CURRENT_DATE) AND CURRENT_DATE
      GROUP BY 2,3,4
      
      UNION ALL
      
      SELECT
          CAST(SUM(I.QUANTIDADE) AS INT) AS QUANTIDADE_ITEM ,
          EXTRACT (MONTH FROM C.CHEGADA) AS MES,
          I.CODITEM AS CODIGO_ITEM,
          I.NOMEITEM AS NOME_ITEM
      FROM CHEGADA C
      JOIN PRODCHEG I ON  (I.CODCHEGADA = C.CODIGO)
      WHERE (1=1)
      and I.DEVOLUCAO <> 'T'
      AND C.CHEGADA BETWEEN DATEADD (MONTH, -12, CURRENT_DATE) AND CURRENT_DATE
      GROUP BY 2,3,4

      UNION ALL
      
      SELECT
          CAST(SUM(I.QUANTIDADE) AS INT) AS QUANTIDADE_ITEM,
          EXTRACT (MONTH FROM C.CHEGADA) AS MES,
          'Serviço' AS CODIGO_ITEM,
          i.nomeserv AS NOME_ITEM
      FROM CHEGADA C
      JOIN servcheg I ON  (I.CODCHEGADA = C.CODIGO)
      WHERE (1=1)
      and I.DEVOLUCAO <> 'T'
      AND C.CHEGADA BETWEEN DATEADD (MONTH, -12, CURRENT_DATE) AND CURRENT_DATE
      GROUP BY 2,3,4
      )

select
    CODIGO_ITEM,
    NOME_ITEM,
    sum(iif(mes = 1, QUANTIDADE_ITEM, 0)) as janeiro,
    sum(iif(mes = 2, QUANTIDADE_ITEM, 0)) as fevereiro,
    sum(iif(mes = 3, QUANTIDADE_ITEM, 0)) as marco,
    sum(iif(mes = 4, QUANTIDADE_ITEM, 0)) as abril,
    sum(iif(mes = 5, QUANTIDADE_ITEM, 0)) as maio,
    sum(iif(mes = 6, QUANTIDADE_ITEM, 0)) as junho,
    sum(iif(mes = 7, QUANTIDADE_ITEM, 0)) as julho,
    sum(iif(mes = 8, QUANTIDADE_ITEM, 0)) as agosto,
    sum(iif(mes = 9, QUANTIDADE_ITEM, 0)) as setembro,
    sum(iif(mes = 10, QUANTIDADE_ITEM, 0)) as outubro,
    sum(iif(mes = 11, QUANTIDADE_ITEM, 0)) as novembro,
    sum(iif(mes = 12, QUANTIDADE_ITEM, 0)) as dezembro
from COMPRA
group by 1,2;with COMPRA as (
      SELECT
          SUM(C.vtotal) AS VALOR,
          EXTRACT (MONTH FROM C.CHEGADA) AS MES,
          C.CONTA AS CONTA
      FROM CHEGADA C
      WHERE (1=1)
      AND C.CHEGADA BETWEEN DATEADD (MONTH, -12, CURRENT_DATE) AND CURRENT_DATE
      GROUP BY 2,3
      )

select
    CONTA,
    sum(iif(mes = 1, VALOR, 0)) as janeiro,
    sum(iif(mes = 2, VALOR, 0)) as fevereiro,
    sum(iif(mes = 3, VALOR, 0)) as marco,
    sum(iif(mes = 4, VALOR, 0)) as abril,
    sum(iif(mes = 5, VALOR, 0)) as maio,
    sum(iif(mes = 6, VALOR, 0)) as junho,
    sum(iif(mes = 7, VALOR, 0)) as julho,
    sum(iif(mes = 8, VALOR, 0)) as agosto,
    sum(iif(mes = 9, VALOR, 0)) as setembro,
    sum(iif(mes = 10, VALOR, 0)) as outubro,
    sum(iif(mes = 11, VALOR, 0)) as novembro,
    sum(iif(mes = 12, VALOR, 0)) as dezembro
from COMPRA
group by 1;select distinct
A.MES || '/' ||A.ANO AS DATA_RECEBIMENTO,
SUM(A.VALOR_TOTAL) AS TOTAL,
A.CONTA,
dense_rank() over(partition by A.mes || '/' || A.ANO order by SUM(A.VALOR_TOTAL) desc) as ranking
from (
       select * from
        ( SELECT  MR.MES AS MES , MR.ANO AS ANO, C.CHEGADA as CHEGADA,
            (C.vtotal) AS VALOR_TOTAL,
             c.conta AS CONTA
        FROM CHEGADA C
          JOIN v_mes_recebimento MR ON (MR.recebimento = C.codigo)
 )
  where (1=1)
  [ comando:AND CHEGADA BETWEEN {0} AND {1}; texto: Chegada; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje ]
   ) A
 GROUP BY 1,3
 order by 1, 4; select distinct
A.CONTA,
SUM(A.VALOR_TOTAL) AS TOTAL,
dense_rank() over( order by SUM(A.VALOR_TOTAL) desc) as ranking
from (
       select * from
        ( SELECT  MR.MES AS MES , MR.ANO AS ANO, C.CHEGADA as CHEGADA,
            (C.vtotal) AS VALOR_TOTAL,
             c.conta AS CONTA
        FROM CHEGADA C
          JOIN v_mes_recebimento MR ON (MR.recebimento = C.codigo)
 )
  where (1=1)
  [ comando:AND CHEGADA BETWEEN {0} AND {1}; texto: Chegada; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje ]
   ) A
 GROUP BY 1
 order by 3;select distinct
    A.ITEM,
    (A.QUANTIDADE) AS QUANTIDADE,
    A.UNIDADE,
    dense_rank() over( order by (A.QUANTIDADE) desc) as ranking
from (
       select item, unidade, sum(quantidade) as quantidade from
            ( SELECT   K.DATA as DATA_CONSUMO,
                (K.quantsai - K.quantent) AS QUANTIDADE,
                 U.CNOME AS UNIDADE,
                 I.cnome AS ITEM
             from kardex k
             JOIN ITENS I ON (I.ccodigo = K.CCODIGO)
             JOIN UNIDADES U ON (U.codigo = I.CUNIDEST)
             where (1=1)
             and i.ativo = 1
             and i.itemprod <> '2'
             and ACAO IN ('RQ','DV')
            )
        where (1=1)
        [ comando:AND DATA_CONSUMO BETWEEN {0} AND {1}; texto: Data Consumo; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje ]
        group by item, unidade
        ) A
 order by 4;
select distinct
    A.ITEM,
    (A.QUANTIDADE) AS QUANTIDADE,
    A.UNIDADE,
    dense_rank() over( order by (A.QUANTIDADE) desc) as ranking
from (
       select item, unidade, sum(quantidade) as quantidade from
            ( SELECT   K.DATA as DATA_CONSUMO,
                (K.quantsai - K.quantent) AS QUANTIDADE,
                 U.CNOME AS UNIDADE,
                 I.cnome AS ITEM
             from kardex k
             JOIN ITENS I ON (I.ccodigo = K.CCODIGO)
             JOIN UNIDADES U ON (U.codigo = I.CUNIDEST)
             where (1=1)
             and i.ativo = 1
             and i.itemprod <> '2'
             and i.tipoitemec = 1
             and ACAO IN ('RQ','DV')
            )
        where (1=1)
        [ comando:AND DATA_CONSUMO BETWEEN {0} AND {1}; texto: Data Consumo; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje ]
        group by item, unidade
        ) A
 order by 4; select distinct
A.FORNECEDOR,
SUM(A.VALOR_TOTAL) AS TOTAL,
dense_rank() over( order by SUM(A.VALOR_TOTAL) desc) as ranking
from (
       select * from
        ( SELECT  MR.MES AS MES , MR.ANO AS ANO, C.CHEGADA as CHEGADA,
            (C.vtotal) AS VALOR_TOTAL,
             c.FORNEC AS FORNECEDOR
        FROM CHEGADA C
          JOIN v_mes_recebimento MR ON (MR.recebimento = C.codigo)
 )
  where (1=1)
  [ comando:AND CHEGADA BETWEEN {0} AND {1}; texto: Chegada; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje ]
   ) A
 GROUP BY 1
 order by 3;select
    p.cnpo as orcamento,
    p.referencia as Titulo,
    p.precofinal as valor,
    p.data as dataOrcamento,
    p.quantidade as Quantidade,
    v.cnome as Vendedor,
    c.cnome as Cliente,
    case when
        c.prospect = 'T' then 'Prospect'
        else 'Cliente' 
	end as tipo

from pridat p
join clientes c on (c.ccodigo = p.codcliente)
join vendedor v on (v.ccodigo = p.codvend1)
where (1=1)
[ comando:and p.data between {0} and {1}; texto: Orçamentos realizados entre; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje]
order by p.data desc, p.cliente;select
    p.cnpo as orcamento,
    p.referencia as Titulo,
    p.precofinal as valor,
    p.data as dataOrcamento,
    p.quantidade as Quantidade,
    v.cnome as Vendedor,
    c.cnome as Cliente,
    case when
        c.prospect = 'T' then 'Prospect'
        else 'Cliente' 
	end as tipo

from pridat p
join clientes c on (c.ccodigo = p.codcliente)
join vendedor v on (v.ccodigo = p.codvend1)
where (1=1)
[ comando:and p.data between {0} and {1}; texto: Orçamentos realizados entre; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje]
order by p.data desc, p.cliente;SELECT
   extract(year from min(poscalculo.data)) as AnoData,
   poscalculo.cliente as cliente,
   sum(poscalculo.MedVlrNegOrc) as precoOrc,
   sum(poscalculo.MedVlrNegOS) as precoOs,
   CASE
      WHEN sum(poscalculo.MedVlrNegOrc) <> 0 THEN sum(poscalculo.MedVlrNegOS) / NULLIF(sum(poscalculo.MedVlrNegOrc), 0)
      ELSE 0
   END as PERCCONVVAL,
   cast(SUM(poscalculo.contadorOS) as int) as contadorOS,
   cast(SUM(poscalculo.contadorOrc) as int) as contadorOrc,
   CASE
      WHEN SUM(poscalculo.contadorOrc) <> 0 THEN CAST((SUM(poscalculo.contadorOS) / NULLIF(SUM(poscalculo.contadorOrc), 0)) * 100 AS DECIMAL(10, 2))
      ELSE 0
   END as PERCONV,
   sum(poscalculo.MedVlrNegOS) / NULLIF(SUM(poscalculo.contadorOS), 0) as MEDVLROS
from(
        select
            1.00 as ContadorOrc,
            0.00 as ContadorOS,
            p.data as data,
            p.CLIENTE  as Cliente,
            0 as MedVlrNegOS,
            (p.precofinal) as MedVlrNegOrc
        from pridat    p 
        join geralcad gc on (gc.empresa = p.empresa)
        join CLIENTES  cf on(cf.CCODIGO = p.CODCOMERCIAL)
        join status    t  on(t.codseq = p.statuscod)
        left join witens wi on (wi.codseq = p.codseqitens)
        left join FORMPAG   f  on(f.CODIGO = p.CODFORMPAG)
        left join VENDEDOR  v1  on (v1.CCODIGO = p.CODVEND1)
        left join VENDEDOR  v2  on (v2.CCODIGO = p.CODVEND2)
        left join TipoServ ts on (ts.CodSeq = p.TipoDeServico)
        left join ctiposer tr on (tr.codseq = p.tipotrab)
        left join endcli ec on (ec.codseq = cf.codendcli)
        where 1=1

        union all
        
        select
            0.00 as ContadorOrc,
            1.00 as ContadorOS,
            a.cdata as data,
            a.ccliente as Cliente,
            a.cpfinal as MedVlrNegOS,
            0 as MedVlrNegOrc
        from arqos    a
        join geralcad gc on (gc.empresa = a.empresa)
        join CLIENTES  cf on(cf.CCODIGO = a.CODCOMERCIAL)
        join status    t  on(t.codseq = a.statuscod)
        left join FORMPAG f  on(f.CODIGO = a.codformpag)
        left join VENDEDOR v1 on (v1.CCODIGO = a.codvend1)
        left join VENDEDOR v2 on (v2.CCODIGO = a.CODVEND2)
        left join TipoServ ts on (ts.CodSeq = a.TipoDeServico)
        left join ctiposer tr on (tr.codseq = a.tipotrab)
        left join endcli ec on (ec.codseq = cf.codendcli)
        where 1 = 1
        and a.cgeraos <> -1
          ) poscalculo
          where (1=1)
          [ comando:and poscalculo.data between {0} and {1}; texto: Data entre; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: inicio_mes_atual; valorPadraoAdicional: fim_mes_atual]
         group by poscalculo.cliente;select    
    m.maquina as maquina,
    e.nome as evento,
    sum(datediff(hour, r.inicio, r.fim)) as tempoParada
from regop r
join maquinas m on (m.codseq = r.maquina)
join eventos e on (e.codseq = r.evento)
where (1=1)
and e.prod = 0
[ comando:r.inicio between {0} and {1}; texto: Data da Parada; tipo: Date; subtipo: dateBetween; nome: filtroDt; valorPadrao: ontem; valorPadraoAdicional: hoje]
group by m.maquina, e.nome";
            var relations = new List<Tuple<string, string>>();
            foreach (var query in queries.Split(';'))
            {
                var parser = new QueryParser();
                relations.AddRange(parser.ExtractRelationships(query));
            }
            //Assert.Equal(4, relations.Count);
            //Assert.Contains(relations,
            //    (rel) => rel.Item1 == "EVENTOS.CODSEQ" && rel.Item2 == "TAPONT.CODEVENTO");
            //Assert.Contains(relations,
            //    (rel) => rel.Item1 == "OPERADOR.CODSEQ" && rel.Item2 == "TAPONT.CODOPERADOR");
            //Assert.Contains(relations,
            //    (rel) => rel.Item1 == "MAQUINAS.CODSEQ" && rel.Item2 == "TAPONT.CODMAQUINA");
            //Assert.Contains(relations,
            //    (rel) => rel.Item1 == "PROCS.CODSEQ" && rel.Item2 == "TAPONT.CODPROC");


            var strutReader = new StrutReader();
            var graphBuilder = new EcalcGraphBuilder();
            var graphExporter = new EcalcGraphExporter();
            var columns = strutReader.GetColumns(@"C:\Users\mathe\source\repos\AntlrDiscountPlatform\AntlrDiscountPlatform\SqlRelantionshipMapper\Parser\Strut.xml");
            var tables = strutReader.GetTables(@"C:\Users\mathe\source\repos\AntlrDiscountPlatform\AntlrDiscountPlatform\SqlRelantionshipMapper\Parser\Strut.xml");

            var graphTables = graphBuilder.BuildTablesGraph(tables, relations);
            var graphColumns = graphBuilder.BuildColumnsGraph(columns, relations);
            graphExporter.ExportTableGraphToGraphviz(graphTables, @"C:\Users\mathe\source\repos\AntlrDiscountPlatform\AntlrDiscountPlatform\SqlRelantionshipMapper\Parser\graph-tables.txt");
            graphExporter.ExportColumnGraphToGraphviz(graphColumns, @"C:\Users\mathe\source\repos\AntlrDiscountPlatform\AntlrDiscountPlatform\SqlRelantionshipMapper\Parser\graph-columns.txt");
        }
    }
}
