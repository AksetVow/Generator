using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Generator.Utils
{
    public static class ViewFactory
    {
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


    }
}
