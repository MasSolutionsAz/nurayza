using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class MenuBannerItems
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int FilesId { get; set; }
        public virtual Files File { get; set; }
        public string Link { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
