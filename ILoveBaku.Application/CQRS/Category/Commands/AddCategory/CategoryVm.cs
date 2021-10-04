using ILoveBaku.Application.Common.Attributes;
using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ILoveBaku.Application.CQRS.Category.Commands.AddCategory
{
    public class CategoryVm : IMapFrom<Categories>
    {
        [Required(ErrorMessage ="Başlıq boş qala bilməz.")]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Boş qala bilməz.")]
        public int? ParentId { get; set; }
        [Required(ErrorMessage = "Öncəlik boş qala bilməz.")]
        public byte? Priority { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
    }
}
