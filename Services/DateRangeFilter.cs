namespace DataExtraction.Services
{
    public class DateRangeFilter
    {
        public string Field { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public DateRangeFilter(string field, int month, int year)
        {
            Field = field;
            StartDate = new DateTime(year, month, 01);
            EndDate = StartDate.AddMonths(1);
        }
        public DateRangeFilter(string field, DateTime startDate, DateTime endDate)
        {
            Field = field;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}