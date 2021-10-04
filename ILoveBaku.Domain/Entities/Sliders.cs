using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class Sliders
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FilesId { get; set; }
    }
}
