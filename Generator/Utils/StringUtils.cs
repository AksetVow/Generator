using System.Text.RegularExpressions;

namespace Generator.Utils
{
    static class StringUtils
    {
        public static bool IsTextNumeric(string str)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(str);
        }
    }
}
