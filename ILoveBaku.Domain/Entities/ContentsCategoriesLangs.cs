using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ContentsCategoriesLangs
    {
        public int Id { get; set; }
        public int ContentsCategoriesId { get; set; }
        public byte LangsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
