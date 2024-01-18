using DatabaseMapper.Core.Parser.Interfaces;
using System;

namespace DatabaseMapper.Core.Parser.Models
{
    public class FilterDateTimeValue : FilterValue, IValueType<DateTime>
    {
        public FilterDateTimeValue(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; internal set; }

        public bool Equals(DateTime other) => Value.Equals(other);
    }
}
