using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class TranslationsGroups
    {
        public byte Id { get; set; }
        public string Path { get; set; }
        public string DisplayName { get; set; }
    }
}
