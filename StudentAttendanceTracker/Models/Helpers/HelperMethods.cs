
//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 
namespace StudentAttendanceTracker.Models.Helpers
{
    public static class HelperMethods
    {
 
        public enum WeekType
        {
            IncludeWeekends,
            ExcludeWeekends,
            
        }
        
        public static string GetRandomCharacters(int characterCount)
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var x = 1;
            var y = 2;

            return new(Enumerable.Repeat(chars, characterCount).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static IEnumerable<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate, WeekType weekType)
        {
            if (endDate < startDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");

            while (startDate <= endDate)
            {
                if ((startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday) && weekType == WeekType.ExcludeWeekends)
                {
                    startDate = startDate.AddDays(1);
                    continue;
                }
                
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }
    }
}
