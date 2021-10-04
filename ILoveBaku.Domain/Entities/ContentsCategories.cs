using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ContentsCategories
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
