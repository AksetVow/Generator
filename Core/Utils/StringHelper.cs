using System.Text;

namespace Core.Utils
{
    static class StringHelper
    {
        public static string Multiply(this string source, int number)
        {
            StringBuilder sb = new StringBuilder(number * source.Length);
            for (int i = 0; i < number; i++)
            {
                sb.Append(source);
            }

            return sb.ToString();
        }

    }
}
