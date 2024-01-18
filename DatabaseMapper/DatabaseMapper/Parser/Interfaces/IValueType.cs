using System;

namespace DatabaseMapper.Core.Parser.Interfaces
{
    internal interface IValueType<T> : IEquatable<T>
    {
        T Value { get; }
    }
}
