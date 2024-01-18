using DatabaseMapper.Core.Parser.Interfaces;
using System;

namespace DatabaseMapper.Core.Parser.Models
{
    public class FilterDecimalValue : FilterValue, IValueType<decimal>
    {
        public FilterDecimalValue(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; internal set; }

        public bool Equals(decimal other) => Value.Equals(other);
    }
}
