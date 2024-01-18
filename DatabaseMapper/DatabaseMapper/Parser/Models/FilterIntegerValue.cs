using DatabaseMapper.Core.Parser.Interfaces;
using System;

namespace DatabaseMapper.Core.Parser.Models
{
    public class FilterIntegerValue : FilterValue, IValueType<int>
    {

        public FilterIntegerValue(int value)
        {
            Value = value;
        }
        
        public int Value { get; internal set; }

        public bool Equals(int other) => Value.Equals(other);

    }
}
