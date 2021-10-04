using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Translations
    {
        public int Id { get; set; }
        public byte TranslationsGroupsId { get; set; }
        public string TransKey { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
