using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataExtraction.Interfaces;

namespace DataExtraction.Services
{
    public class DataTableToJsonConvertor:IDataTableToJsonConvertor
    {
        public string DataTableToJson(DataTable dataTable)
        {
            var rows = new List<Dictionary<string, object?>>(dataTable.Rows.Count);
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object?>(dataTable.Columns.Count, StringComparer.OrdinalIgnoreCase);
                foreach (DataColumn c in dataTable.Columns)
                    dict[c.ColumnName] = row[c] is DBNull ? null : row[c];
                rows.Add(dict);
            }
            return System.Text.Json.JsonSerializer.Serialize(rows,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = false });
        }

    }
}