using ILoveBaku.Application.CQRS.Menus.Queries.GetMenu;
using ILoveBaku.Application.CQRS.Menus.Queries.GetMenuLangs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ILoveBaku.Application.CQRS.Menus.Queries.GetMenus
{
    public class MenuItemDto
    {
        public MenuItemDto()
        {
            Banners = new List<MenuBannerItemDto>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public int ParentId { get; set; }
        [Required]
        public int MenuTypeId { get; set; }
        [Required]
        public int CategoryParentId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string CategoryRootName { get; set; }
        public bool IsActive { get; set; }
        public bool IsManualLink { get; set; }
        public List<MenuItemDto> Children { get; set; }
        public List<MenuLangDto> MenuLangs { get; set; }
        public List<MenuBannerItemDto> Banners { get; set; }

    }
}