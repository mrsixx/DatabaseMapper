using System;
using static DatabaseMapper.Core.Parser.Enums.FilterTypeEnum;

namespace DatabaseMapper.Core.Parser.Exceptions
{
    public class InvalidFunctionTypeException : Exception
    {
        public InvalidFunctionTypeException(string name, FilterType type) : base($"Função {name} deve ser atribuída a um filtro de tipo ${type}") { }
    }
}
