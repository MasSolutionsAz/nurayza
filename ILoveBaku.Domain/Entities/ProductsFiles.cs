using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Entities
{
    public class ProductsFiles
    {
        public int Id { get; set; }
        public virtual Products Products { get; set; }
        public int ProductsId { get; set; }

        public int FilesId { get; set; }

        public virtual Files Files { get; set; }

        public bool IsMain { get; set; }
    }
}
