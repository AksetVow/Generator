using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Generator.Utils
{

    class MenuItemsJson
    {
        public IEnumerable<string> Region { get; set; }
        public IEnumerable<string> Theme { get; set; }
    }

    static class MenuItemsReader
    {
        static MenuItemsJson _menuItems;

        static MenuItemsReader()
        {
            string menuItemsJson = File.ReadAllText("menuitems.txt");
            _menuItems = JsonConvert.DeserializeObject<MenuItemsJson>(menuItemsJson);
        }

        public static IEnumerable<string> GetRegionItems()
        {
            return _menuItems == null ? null : _menuItems.Region;
        }

        public static IEnumerable<string> GetThemeItems()
        {
            return _menuItems == null ? null : _menuItems.Theme;
        }


        public static void SaveItems(IEnumerable<string> regionItems, IEnumerable<string> themeItems)
        {
            var toJsonObject = new MenuItemsJson (){ Region = regionItems, Theme = themeItems };
            string serialized = JsonConvert.SerializeObject(toJsonObject);
            File.WriteAllText("menuitems.txt", serialized);
        }
    }
}
