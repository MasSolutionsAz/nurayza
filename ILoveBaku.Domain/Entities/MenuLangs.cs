using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class MenuLangs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public byte LangsId { get; set; }
        public virtual Langs Lang { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
