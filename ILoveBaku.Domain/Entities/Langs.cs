using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Langs
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Culture { get; set; }
        public byte LangsStatusesId { get; set; }
        public byte Priority { get; set; }
        public virtual Files Files { get; set; }
        public int FilesId { get; set; }
    }
}
