using DatabaseMapper.Core.Parser.Interfaces;
using static DatabaseMapper.Core.Parser.Enums.DefaultValueFuncionEnum;

namespace DatabaseMapper.Core.Parser.Models
{
    public class FilterFunctionValue : FilterValue, IValueType<DefaultValueFunction>
    {
        public FilterFunctionValue(DefaultValueFunction value)
        {
            Value = value;
        }

        public DefaultValueFunction Value { get; internal set; }

        public bool Equals(DefaultValueFunction other) => Value == other;

    }
}