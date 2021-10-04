using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class NewsFiles
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public int Priority { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedDate { get; set; }

        public int FilesId { get; set; }

        public virtual Files Files { get; set; }
    }
}
