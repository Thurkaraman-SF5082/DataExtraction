using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataExtraction.Models
{
    public class QueryReader
    {
        public string ReadQuery(string fileName)
        {
            return File.ReadAllText(Path.Combine("SqlQueries", fileName));
        }
    }
}