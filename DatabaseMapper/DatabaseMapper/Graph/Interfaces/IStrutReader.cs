﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseMapper.Core.Graph.Interfaces
{
    public interface IStrutReader
    {
        IEnumerable<string> GetColumns(string filePath);
        IEnumerable<string> GetTables(string filePath);
    }
}
