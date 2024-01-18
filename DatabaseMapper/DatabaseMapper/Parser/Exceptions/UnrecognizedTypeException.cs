using System;

namespace DatabaseMapper.Core.Parser.Exceptions
{
    public class UnrecognizedTypeException : Exception
    {
        public UnrecognizedTypeException(string filterName) : base($"Filtro {filterName} tipo de valor default não reconhecido.") { }
    }
}
