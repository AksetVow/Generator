using HtmlAgilityPack;

namespace Core.Utils
{
    public static class HtmlTextHelper
    {
        public const string QuatationMarkFromHtml = "&quot;";
        public const string QuatationMark = "\"";
        public const string TagsBeginning = "<";

        public static string CorrectTextFromHtml(string text)
        {
            string result = "";

            if (text.Contains(TagsBeginning))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(text);

                foreach (HtmlNode node in doc.DocumentNode.ChildNodes)
                {
                    result += node.InnerText;
                }
            }
            else
            {
                result = text;
            }

            result = result.Replace("&quot;", "\"");
            result = result.Trim();


            return result;
        }


    }
}
