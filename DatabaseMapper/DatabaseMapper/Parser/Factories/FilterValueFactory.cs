using Antlr4.Runtime.Tree;
using DatabaseMapper.Core.Parser.Enums;
using DatabaseMapper.Core.Parser.Exceptions;
using DatabaseMapper.Core.Parser.Interfaces;
using DatabaseMapper.Core.Parser.Models;
using System;
using System.Runtime.InteropServices;
using static DatabaseMapper.Core.Parser.Enums.DefaultValueFuncionEnum;
using static DatabaseMapper.Core.Parser.TSqlParser;

namespace DatabaseMapper.Core.Parser.Factories
{
    internal class FilterValueFactory : IFilterValueFactory
    {
        public FilterValue GetValue(Filter filter, TSqlParser.Efilter_default_expressionContext defaultExpressionCtx)
        {
            // funções 
            if (defaultExpressionCtx.GetChild(1) is TerminalNodeImpl functionValue)
            {
                var strValue = functionValue.GetText();
                switch (strValue.ToUpperInvariant())
                {
                    case "USUARIO":
                        if (filter.Type != FilterTypeEnum.FilterType.Integer)
                            throw new InvalidFunctionTypeException(strValue, FilterTypeEnum.FilterType.Integer);
                        return new FilterFunctionValue(DefaultValueFunction.Usuario);
                    case "DIAATUAL":
                        if (filter.Type != FilterTypeEnum.FilterType.Integer)
                            throw new InvalidFunctionTypeException(strValue, FilterTypeEnum.FilterType.Integer);
                        return new FilterFunctionValue(DefaultValueFunction.DiaAtual);
                    case "MESATUAL":
                        if (filter.Type != FilterTypeEnum.FilterType.Integer)
                            throw new InvalidFunctionTypeException(strValue, FilterTypeEnum.FilterType.Integer);
                        return new FilterFunctionValue(DefaultValueFunction.MesAtual);
                    case "ANOATUAL":
                        if (filter.Type != FilterTypeEnum.FilterType.Integer)
                            throw new InvalidFunctionTypeException(strValue, FilterTypeEnum.FilterType.Integer);
                        return new FilterFunctionValue(DefaultValueFunction.AnoAtual);
                    case "INICIO_MES_ATUAL":
                        if (filter.Type != FilterTypeEnum.FilterType.Date && filter.Type != FilterTypeEnum.FilterType.DateTime)
                            throw new InvalidFunctionTypeException(strValue, FilterTypeEnum.FilterType.DateTime);
                        return new FilterFunctionValue(DefaultValueFunction.InicioMesAtual);
                    case "FIM_MES_ATUAL":
                        if (filter.Type != FilterTypeEnum.FilterType.Date && filter.Type != FilterTypeEnum.FilterType.DateTime)
                            throw new InvalidFunctionTypeException(strValue, FilterTypeEnum.FilterType.DateTime);
                        return new FilterFunctionValue(DefaultValueFunction.FimMesAtual);
                    case "ONTEM":
                        if (filter.Type != FilterTypeEnum.FilterType.Date && filter.Type != FilterTypeEnum.FilterType.DateTime)
                            throw new InvalidFunctionTypeException(strValue, FilterTypeEnum.FilterType.DateTime);
                        return new FilterFunctionValue(DefaultValueFunction.Ontem);
                    case "HOJE":
                        if (filter.Type != FilterTypeEnum.FilterType.Date && filter.Type != FilterTypeEnum.FilterType.DateTime)
                            throw new InvalidFunctionTypeException(strValue, FilterTypeEnum.FilterType.DateTime);
                        return new FilterFunctionValue(DefaultValueFunction.Hoje);
                    default: throw new UnrecognizedTypeException(functionValue.GetText());
                }
            }
            // valores constantes
            if(defaultExpressionCtx.GetChild(1) is ConstantContext constant)
            {
                var strValue = constant.GetText();
                switch (filter.Type)
                {
                    case FilterTypeEnum.FilterType.Integer:
                        if(Int32.TryParse(strValue, out int @int))
                            return new FilterIntegerValue(@int);
                        throw new MismatchedTypesException(strValue, typeof(int));
                    case FilterTypeEnum.FilterType.Decimal:
                        if (Decimal.TryParse(strValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal @decimal))
                            return new FilterDecimalValue(@decimal);
                        throw new MismatchedTypesException(strValue, typeof(decimal));
                    case FilterTypeEnum.FilterType.Date:
                    case FilterTypeEnum.FilterType.DateTime:
                        if (DateTime.TryParse(strValue.Replace("\'", String.Empty), out DateTime @date))
                            return new FilterDateTimeValue(@date);
                        throw new MismatchedTypesException(strValue, typeof(DateTime));
                    case FilterTypeEnum.FilterType.Text:
                        return new FilterTextValue(strValue.Replace("\'", String.Empty));
                    case FilterTypeEnum.FilterType.Undefined:
                    default:
                        throw new NotImplementedException($"{filter.Type} não implementado.");
                }
            }

            throw new UnrecognizedTypeException(filter.Name);
        }
    }
}
