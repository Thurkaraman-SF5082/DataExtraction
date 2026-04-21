using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataExtraction.Interfaces
{
    public interface IDBService
    {
        Task<DataTable> QueryRouter();
    }
}