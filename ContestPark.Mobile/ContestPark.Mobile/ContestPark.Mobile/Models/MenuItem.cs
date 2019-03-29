using System.Collections.Generic;

namespace ContestPark.Mobile.Models
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string PageName { get; set; }
    }

    public class MenuItemList : List<MenuItem>
    {
        public MenuItemList()
        {
        }

        public MenuItemList(string heading)
        {
            Heading = heading;
        }

        public string Heading { get; set; }
        public List<MenuItem> Persons => this;
    }
}