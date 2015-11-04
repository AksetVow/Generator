using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Utils
{
    public static class MatchHelper
    {
        public static string SelectResultValue(Match match)
        {
            if (match.Groups.Count > 1)
            {
                string result;
                for (int i = match.Groups.Count; i > 0; i--)
                {
                    result = match.Groups[i].Value;
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public static void ProcessImages(MatchCollection matches, IList<string> images)
        {
            string image;
            for (int i = 0; i < matches.Count; i++)
            {
                image = MatchHelper.SelectResultValue(matches[i]);
                if (image != null)
                {
                    images.Add(image);
                }
            }
        
        }

    }
}
