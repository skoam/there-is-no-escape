using System.Text;

namespace Fumiko.Common
{
    public static class StringExtensions
    {
        public static string ToBase64(this string inputString)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(inputString);
            string base64String = System.Convert.ToBase64String(plainTextBytes);
            return base64String;
        }

        public static string FromBase64(this string inputString)
        {
            var plainTextBytes = System.Convert.FromBase64String(inputString);
            string decodedString = Encoding.UTF8.GetString(plainTextBytes);
            return decodedString;
        }
    }
}