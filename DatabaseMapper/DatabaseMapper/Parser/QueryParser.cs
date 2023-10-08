using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;

namespace DatabaseMapper.Parser
{
    public class QueryParser
    {
        public List<Tuple<string, string>> ExtractRelationships(string query)
        {

            var reader = new StringReader(query);
            var charStream = new AntlrInputStream(reader);
            var lexer = new TSqlLexer(new CaseChangingCharStream(charStream, true));
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new TSqlParser(tokenStream);

            var fileCtx = parser.tsql_file();
            // Walk it and attach our listener
            var walker = new ParseTreeWalker();
            var listener = new TSqListener();
            walker.Walk(listener, fileCtx);
            return listener.Relations;
        }
    }
}
