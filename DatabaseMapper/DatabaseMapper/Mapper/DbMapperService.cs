using DatabaseMapper.Graph;
using DatabaseMapper.Mapper.Interfaces;
using DatabaseMapper.Parser;
using DatabaseMapper.Parser.Interfaces;
using System.Linq;

namespace DatabaseMapper.Mapper
{
    public class DbMapperService : IDbMapperService
    {
        public void IncrementModel(TableGraph model, string query)
        {
            var queryParser = new QueryParser(query);
            var tables = queryParser.ExtractTables();
            var relationships = queryParser.ExtractRelationships();

            foreach (var table in tables.Values)
                model.AddVertex(table);

            var vertices = model.Vertices.ToList();
            foreach (var relation in relationships)
            {

                var source = relation.Item1.Split('.');
                var target = relation.Item2.Split('.');
                var idx1 = vertices.FindIndex(v => v.Table == source[0]);
                var idx2 = vertices.FindIndex(v => v.Table == target[0]);
                if (idx1 != -1 && idx2 != -1)
                {
                    var e1 = vertices[idx1];
                    var e2 = vertices[idx2];
                    var edge = new TableGraphEdge(e1, e2, source[1], target[1]);
                    if (!model.ContainsEdge(edge))
                        model.AddEdge(edge);
                }
            }
        }
    }
}
