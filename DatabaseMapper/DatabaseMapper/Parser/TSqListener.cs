﻿using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DatabaseMapper.Core.Parser.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static DatabaseMapper.Core.Parser.TSqlParser;

namespace DatabaseMapper.Core.Parser
{
    public class TSqListener : TSqlParserBaseListener
    {
        public string OriginalQuery { get; }
        public Dictionary<string, string> TableAliases { get; }

        public QueryMetadata Metadata { get; }

        public TSqListener(string query)
        {
            OriginalQuery = query;
            Metadata = new QueryMetadata();
            TableAliases = new Dictionary<string, string>();
        }

        public override void EnterTable_source_item([NotNull] TSqlParser.Table_source_itemContext ctx)
        {
            if (!ctx.IsEmpty && ctx.GetChild(0) is TSqlParser.Full_table_nameContext tableNameContext)
            {
                var currentTableName = tableNameContext.GetText().ToUpperInvariant();
                if (ctx.ChildCount > 1 && ctx.GetChild(1) is TSqlParser.As_table_aliasContext aliasContext)
                {
                    var alias = aliasContext.ChildCount > 1 ? aliasContext.GetChild(1).GetText().ToUpperInvariant() : aliasContext.GetText().ToUpperInvariant();
                    if (!TableAliases.ContainsKey(alias))
                        TableAliases.Add(alias, currentTableName);
                }
                else if (!TableAliases.ContainsKey(currentTableName))
                    TableAliases.Add(currentTableName, currentTableName);


                var table = Metadata.Tables.Find(t => t.TableName == currentTableName);
                if (table is null)
                {
                    table = new QueryTable(currentTableName);
                    Metadata.Tables.Add(table);
                }
                table.IncrementOcurrencies();
            }
        }

        public override void EnterPredicate([NotNull] TSqlParser.PredicateContext context)
        {
            if (context.ChildCount == 3)
            {
                var child1 = context.GetChild(0);
                var child2 = context.GetChild(1);
                var child3 = context.GetChild(2);
                if (child1 is TSqlParser.ExpressionContext expl &&
                   child2 is TSqlParser.Comparison_operatorContext op &&
                   child3 is TSqlParser.ExpressionContext expr)
                {
                    if (expl.GetChild(0) is TSqlParser.Full_column_nameContext leftColumn && expr.GetChild(0) is TSqlParser.Full_column_nameContext rigthColumn)
                    {
                        string leftColumnName = ExtractColumnName(leftColumn);
                        string rigthColumnName = ExtractColumnName(rigthColumn);
                        if (!String.IsNullOrWhiteSpace(leftColumnName) && !String.IsNullOrWhiteSpace(rigthColumnName))
                            Metadata.Relations.Add(new QueryRelation(leftColumnName, rigthColumnName));
                    }
                }
            }
        }

        public override void EnterSelect_statement([NotNull] TSqlParser.Select_statementContext context)
        {
            TableAliases.Clear();
        }

        public override void ExitSelect_statement([NotNull] TSqlParser.Select_statementContext context)
        {
            var query = OriginalQuery;
            Metadata.EcalcFilters.ForEach(f => query = query.Replace(f.Text, String.Empty).Trim());
            Metadata.RealQuery = query;

            if (Metadata.Relations.Count <= 0)
                return;

            Debug.WriteLine("Aliases: ");
            foreach (var tableName in TableAliases.Keys)
                Debug.WriteLine($"{tableName} -> {TableAliases[tableName]}");

            Debug.WriteLine("Relations: ");
            foreach (var relation in Metadata.Relations)
                Debug.WriteLine($"{relation.LeftTable} <-> {relation.RightTable}");
        }

        public override void EnterEfilter_statement([NotNull] TSqlParser.Efilter_statementContext context)
        {
            if (!context.IsEmpty && context.GetChild(1) is TerminalNodeImpl filterName && context.GetChild(2) is TSqlParser.Data_typeContext dataType)
            {
                var ecalcFilter = new EcalcFilter { Name = filterName.GetText(), Type = dataType.GetText() };
                ecalcFilter.Text = context.Start.InputStream.GetText(new Interval(context.Start.StartIndex, context.Stop.StopIndex));
                if (context.children.Any(c => c is As_column_aliasContext))
                {
                    var asColumnAliasCtx = context.GetRuleContext<As_column_aliasContext>(0);
                    ecalcFilter.Alias = asColumnAliasCtx.GetChild<Column_aliasContext>(0).GetText().Replace("\'", String.Empty);
                }

                if (context.children.Any(c => c is Default_expressionContext))
                {
                    var asColumnAliasCtx = context.GetRuleContext<Default_expressionContext>(0);
                    ecalcFilter.DefaultValue = asColumnAliasCtx.GetChild(1).GetText();
                }

                Metadata.EcalcFilters.Add(ecalcFilter);

            }
        }

        /// <summary>
        /// Monta o nome completo da coluna NOMETABELA.NOMECOLUNA
        /// </summary>
        /// <param name="fullColumnNameCtx"></param>
        /// <returns></returns>
        private string ExtractColumnName(TSqlParser.Full_column_nameContext fullColumnNameCtx)
        {
            var strEmpty = "\"\"";
            var columnName = fullColumnNameCtx.GetText().ToUpperInvariant();

            if (columnName == strEmpty) return null;
            if (fullColumnNameCtx.ChildCount < 3) return null;

            if (fullColumnNameCtx.GetChild(0) is TSqlParser.Full_table_nameContext tblNameCtx && fullColumnNameCtx.GetChild(2) is TSqlParser.Id_Context colNameCtx)
            {
                var alias = tblNameCtx.GetText().ToUpperInvariant();
                if (!TableAliases.TryGetValue(alias, out string tblName)) return null;
                return $"{tblName}.{colNameCtx.GetText()}".ToUpperInvariant();
            }

            return null;
        }

    }
}
