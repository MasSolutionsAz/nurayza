using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Entities
{
    public class CategoriesFiles
    {
        public int Id { get; set; }
        public int CategoriesId { get; set; }
        public virtual Categories Categories { get; set; }
        public virtual Files Files { get; set; }
        public int  FilesId { get; set; }
    }
}
