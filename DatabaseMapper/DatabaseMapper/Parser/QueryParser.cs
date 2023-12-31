using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DatabaseMapper.Core.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace DatabaseMapper.Core.Parser
{
    public class QueryParser : IQueryParser
    {
        private readonly string _query;
        private readonly ParseTreeWalker _walker;

        public QueryParser(string query)
        {
            _query = query;
            _walker = new ParseTreeWalker();
        }

        public QueryMetadata ExtractMetadata()
        {
            var reader = new StringReader(_query);
            var charStream = new CaseChangingCharStream(new AntlrInputStream(reader), true);
            var lexer = new TSqlLexer(charStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new TSqlParser(tokenStream);
            var fileCtx = parser.tsql_file();
            var listener = new TSqListener(_query);
            _walker.Walk(listener, fileCtx);
            return listener.Metadata;
        }
    }
}
