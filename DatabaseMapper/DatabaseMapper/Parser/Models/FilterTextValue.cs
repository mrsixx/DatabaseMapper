using DatabaseMapper.Core.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseMapper.Core.Parser.Models
{
    public class FilterTextValue : FilterValue, IValueType<string>
    {
        public FilterTextValue(string strValue)
        {
            Value = strValue;
        }

        public string Value { get; internal set; }

        public bool Equals(string other) => Value.Equals(other);
    }
}
