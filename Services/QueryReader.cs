namespace DataExtraction.Services
{
    public class QueryReader
    {
        public string ReadQuery(string fileName)
        {
            return File.ReadAllText(Path.Combine("SqlQueries", fileName));
        }
    }
}