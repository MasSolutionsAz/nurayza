using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class FilesFolders
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public bool IsAllowDelete { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
