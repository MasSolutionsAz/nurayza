using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class SlidersLangs
    {
        public int Id { get; set; }
        public byte LangsId { get; set; }
        public int SlidersId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}
