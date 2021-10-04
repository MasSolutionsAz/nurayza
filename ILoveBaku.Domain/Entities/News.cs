using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public DateTime ShowDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<NewsFiles> NewsFiles { get; set; }
    }
}
