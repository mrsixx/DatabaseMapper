using DatabaseMapper.Core.Graph;
using DatabaseMapper.Core.Mapper.Interfaces;
using DatabaseMapper.Core.Parser;
using System.Linq;

namespace DatabaseMapper.Core.Mapper
{
    public class DbMapperService : IDbMapperService
    {
        public void IncrementModel(TableGraph model, string query)
        {
            var queryParser = new QueryParser(query);
            var queryMetadata = queryParser.ExtractMetadata();
            var tables = queryMetadata.Tables;
            var relationships = queryMetadata.Relations;

            foreach (var table in tables)
                if(model.Vertices.ToList().FindIndex(v => v.Table == table.TableName) == -1)
                    model.AddVertex(table.TableName);

            var vertices = model.Vertices.ToList();
            foreach (var relation in relationships)
            {

                var source = relation.LeftTable.Split('.');
                var target = relation.RightTable.Split('.');
                var sourceColumnName = source.Last();
                var targetColumnName = target.Last();

                var sourceTblName = relation.LeftTable.Replace($".{sourceColumnName}", string.Empty);
                var targetTblName = relation.RightTable.Replace($".{targetColumnName}", string.Empty);
                var idx1 = vertices.FindIndex(v => v.Table == sourceTblName);
                var idx2 = vertices.FindIndex(v => v.Table == targetTblName);
                if (idx1 != -1 && idx2 != -1)
                {
                    var e1 = vertices[idx1];
                    var e2 = vertices[idx2];
                    var edge = new TableGraphEdge(e1, e2, sourceColumnName, targetColumnName);
                    if (!model.ContainsEdge(edge))
                        model.AddEdge(edge);
                }
            }
        }
    }
}
