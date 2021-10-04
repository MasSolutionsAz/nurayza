using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class FilesTypes
    {
        public int Id { get; set; }
        public byte FilesTypesGroupsId { get; set; }
        public string Name { get; set; }
    }
}
