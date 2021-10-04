using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class NewsKeywords
    {
        public int Id { get; set; }
        public int NewsLangsId { get; set; }
        public int KeywordsId { get; set; }
        public bool IsActive { get; set; }
    }
}
