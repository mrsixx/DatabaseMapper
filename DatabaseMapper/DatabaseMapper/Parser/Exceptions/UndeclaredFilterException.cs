using System;

namespace DatabaseMapper.Core.Parser.Exceptions
{
    public class UndeclaredFilterException : Exception
    {
        public UndeclaredFilterException(string filterName) : base($"Filtro {filterName} não foi declarado.") { }
    }
}
