using DatabaseMapper.Core.Parser.Models;
using static DatabaseMapper.Core.Parser.Enums.FilterTypeEnum;
using static DatabaseMapper.Core.Parser.TSqlParser;

namespace DatabaseMapper.Core.Parser.Interfaces
{
    internal interface IFilterValueFactory
    {
        FilterValue GetValue(Filter filter, Efilter_default_expressionContext defaultExpressionCtx);
    }
}
