using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class CategoriesLangs
    {
        public int Id { get; set; }
        public int CategoriesId { get; set; }
        public virtual Categories Category { get; set; }
        public byte LangsId { get; set; }
        public virtual Langs Lang { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
