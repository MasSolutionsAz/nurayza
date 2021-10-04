using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Menus.Models
{
    public class MenuDto
    {
        public MenuDto()
        {
            MenuItems = new List<MenuItem>();
        }

        public List<MenuItem> MenuItems { get; set; }
    }

    public class MenuItem
    {
        public MenuItem()
        {
            MenuCategories = new List<MenuCategory>();
            MenuBanners = new List<MenuBanner>();
        }

        public string Title { get; set; }

        public string Link { get; set; }

        public List<MenuCategory> MenuCategories { get; set; }

        public List<MenuBanner> MenuBanners { get; set; }
    }

    public class MenuCategory
    {
        public MenuCategory()
        {
            Children = new List<MenuCategory>();
        }

        public string Title { get; set; }

        public string Link { get; set; }

        public List<MenuCategory> Children { get; set; }
    }

    public class MenuBanner
    {
        public string Image { get; set; }

        public string Link { get; set; }
    }
}
