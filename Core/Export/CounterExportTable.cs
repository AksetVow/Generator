using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Export
{
    static class CounterExportTable
    {
        public const string SpecialKeyValue = "#######0bac56da-82e0-4da4-a508-08ab1214a90e#######";

        public static int CountSymbols(IEnumerable<Article> article)
        {
            return 0;
        }

        public static bool IsEqual(Article first, Article second, ExportCounterSettings settings)
        {

            return true;
        }

        private static void ProcessGroupItem(IList<Tuple<string, int, int, bool>> items, string keyProperty, IEnumerable<Article> articles, int count, bool isLast)
        {
            int countSymbols = CountSymbols(articles);
            var resItem = new Tuple<string, int, int, bool>(keyProperty, count, countSymbols, isLast);
            items.Add(resItem);
        }

        public static IList<Tuple<string, int, int, bool>> GetCountArticleTable(IList<Article> articles, ExportCounterSettings settings)
        {
            var grouped = articles.GroupBy(x => new {
                Author = (settings.Author ? x.Author : SpecialKeyValue),
                Category = (settings.Category ? x.Category : SpecialKeyValue),
                SubjectCategory = (settings.SubjectCategory ? x.SubjectCategory : SpecialKeyValue),
                PublicDate = (settings.PublicationDate ? x.PublicDate : SpecialKeyValue),
                Source = (settings.Source ? x.Source : SpecialKeyValue),
                Title = (settings.Title ? x.Title : SpecialKeyValue)
            });

            var result = new List<Tuple<string, int, int, bool>>();
             

            foreach (var item in grouped)
            {
                if (item.Key.Author != SpecialKeyValue)
                {
                    ProcessGroupItem(result, item.Key.Author, item, item.Count(),
                        item.Key.Category == SpecialKeyValue &&
                        item.Key.SubjectCategory == SpecialKeyValue &&
                        item.Key.PublicDate == SpecialKeyValue &&
                        item.Key.Source == SpecialKeyValue &&
                        item.Key.Title == SpecialKeyValue 
                        );
                }
                if (item.Key.Category != SpecialKeyValue)
                {
                    ProcessGroupItem(result, item.Key.Category, item, item.Count(),
                        item.Key.SubjectCategory == SpecialKeyValue &&
                        item.Key.PublicDate == SpecialKeyValue &&
                        item.Key.Source == SpecialKeyValue &&
                        item.Key.Title == SpecialKeyValue);
                }
                if (item.Key.SubjectCategory != SpecialKeyValue)
                {
                    ProcessGroupItem(result, item.Key.SubjectCategory, item, item.Count(),
                        item.Key.PublicDate == SpecialKeyValue &&
                        item.Key.Source == SpecialKeyValue &&
                        item.Key.Title == SpecialKeyValue                        
                        );
                }
                if (item.Key.PublicDate != SpecialKeyValue)
                {
                    ProcessGroupItem(result, item.Key.PublicDate, item, item.Count(),
                        item.Key.Source == SpecialKeyValue &&
                        item.Key.Title == SpecialKeyValue
                        );
                }
                if (item.Key.Source != SpecialKeyValue)
                {
                    ProcessGroupItem(result, item.Key.Source, item, item.Count(), item.Key.Title == SpecialKeyValue);
                }
                if (item.Key.Title != SpecialKeyValue)
                {
                    ProcessGroupItem(result, item.Key.Title, item, item.Count(), true);
                }

            }

            return result;
        }

    }
}
