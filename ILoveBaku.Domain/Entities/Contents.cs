using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Contents
    {
        public int Id { get; set; }
        public int ContentsCategoriesId { get; set; }
        public string Title { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? FilesId { get; set; }
        public virtual Files File { get; set; }
    }
}
