using ILoveBaku.Domain.Enums;
using System;

namespace ILoveBaku.Domain.Entities
{
    public partial class NewsLangs
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public string Description { get; set; }
        public string ContentHtml { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int NewsId { get; set; }

        public virtual News News { get; set; }

        public byte NewsLangsStatusesId { get; set; }

        public byte LangsId { get; set; }

        public virtual Langs Lang { get; set; }
    }
}
