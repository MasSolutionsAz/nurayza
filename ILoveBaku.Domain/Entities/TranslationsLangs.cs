using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class TranslationsLangs
    {
        public int Id { get; set; }
        public int TranslationsId { get; set; }
        public byte LangsId { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
