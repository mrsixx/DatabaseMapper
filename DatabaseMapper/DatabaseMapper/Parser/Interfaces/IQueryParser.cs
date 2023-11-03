using System;
using System.Collections.Generic;

namespace DatabaseMapper.Core.Parser.Interfaces
{
    public interface IQueryParser
    {
        QueryMetadata ExtractMetadata();
    }
}
