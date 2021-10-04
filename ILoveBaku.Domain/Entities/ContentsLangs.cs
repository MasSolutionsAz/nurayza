using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ContentsLangs
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ContentHtml { get; set; }
        public int VisitorCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int ContentsId { get; set; }

        public virtual Contents Content { get; set; }

        public byte LangsId { get; set; }

        public virtual Langs Lang { get; set; }
    }
}
