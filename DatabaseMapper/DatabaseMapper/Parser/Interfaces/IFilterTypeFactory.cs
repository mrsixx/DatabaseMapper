using static DatabaseMapper.Core.Parser.Enums.FilterTypeEnum;

namespace DatabaseMapper.Core.Parser.Interfaces
{
    public interface IFilterTypeFactory
    {
        FilterType GetType(string typeName);
    }
}
