using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Entities
{
    public class Files
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long ContentLength { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FilesFoldersId { get; set; }
        public virtual FilesFolders FileFolder { get; set; }
        public int FilesTypesId { get; set; }
        public virtual FilesTypes FileTypes { get; set; }
    }
}