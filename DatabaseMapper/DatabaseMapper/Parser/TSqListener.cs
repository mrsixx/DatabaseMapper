using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DatabaseMapper.Parser
{
    public class TSqListener : TSqlParserBaseListener
    {
        public Dictionary<string, string> TableAliases { get; }
        public List<Tuple<string, string>> Relations { get; }

        public TSqListener()
        {
            TableAliases = new Dictionary<string, string>();
            Relations = new List<Tuple<string, string>>();
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
                            Relations.Add(new Tuple<string, string>(leftColumnName, rigthColumnName));
                    }
                }
            }
        }

        public override void EnterSelect_statement([NotNull] TSqlParser.Select_statementContext context)
        {
            TableAliases.Clear();
            Relations.Clear();

        }

        public override void ExitSelect_statement([NotNull] TSqlParser.Select_statementContext context)
        {
            if (Relations.Count <= 0)
                return;

            Debug.WriteLine("Aliases: ");
            foreach (var tableName in TableAliases.Keys)
                Debug.WriteLine($"{tableName} -> {TableAliases[tableName]}");

            Debug.WriteLine("Relations: ");
            foreach (var relation in Relations)
                Debug.WriteLine($"{relation.Item1} <-> {relation.Item2}");
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
