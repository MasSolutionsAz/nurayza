using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Menu
    {
        public Menu()
        {
            MenuBannerItems = new HashSet<MenuBannerItems>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public int? ParentId { get; set; }
        public int? Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte MenuTypesId { get; set; }
        public bool IsActive { get; set; }
        public virtual MenuCategoriesItems MenuCategoriesItems { get; set; }
        public virtual ICollection<MenuBannerItems> MenuBannerItems { get; set; }
    }
}
