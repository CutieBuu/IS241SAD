//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12
using Microsoft.IdentityModel.Tokens;

namespace StudentAttendanceTracker.Models.Helpers
{

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) => !input.IsNullOrEmpty() ? string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1)) : "";

    }
}
