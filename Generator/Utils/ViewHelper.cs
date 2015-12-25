using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Generator.Utils
{
    public enum Color
    { 
        Default = 0,
        Yellow = 1,
        Blue = 2
    }


    static class ViewHelper
    {
        private static Color _color = Color.Blue;


        public static MenuItem CreateMenuItem(ICommand command, string header)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Command = command;
            menuItem.CommandParameter = header;
            menuItem.Header = header;

            return menuItem;
        }

        public static IList<string> MenuItems(MenuItem menuItem)
        {
            var items = new List<string>();

            for (int i = 2; i < menuItem.Items.Count; i++)
            {
                var item = menuItem.Items[i] as MenuItem;
                if (item != null)
                {
                    items.Add(item.Header.ToString());
                }
            }

            return items;
        }

        public static void Colorize(IList<Article> articles)
        {
            bool equalSequency = false;

            for (int i = 0; i < articles.Count - 1; i++)
            {
                if (Equals(articles[i], articles[i + 1]))
                {
                    if (!equalSequency)
                    {
                        equalSequency = true;
                        _color = SwitchColor(_color);
                    }
                    articles[i].Color = (int)_color;
                    articles[i+1].Color = (int)_color;

                }
                else
                {
                    equalSequency = false;
                }
            }
        }

        public static void Uncolorize(IList<Article> articles)
        {
            for (int i = 0; i < articles.Count; i++)
            {
                articles[i].Color = (int)Color.Default;
            }
        }

        private static bool Equals(Article first, Article second)
        {
            if (first.Title == null || second.Title == null)
                return false;

            return first.Title.ToUpper().Equals(second.Title.ToUpper());
        }

        private static Color SwitchColor(Color color)
        {
            if (color == Color.Blue)
                return Color.Yellow;
            if (color == Color.Yellow)
                return Color.Blue;

            return Color.Default;
        }

    }
}
