using System;
using System.ComponentModel.DataAnnotations;

namespace ILoveBaku.Application.CQRS.News.Models
{
    public class NewsVm
    {
        [Required(ErrorMessage = "Başlıq boş qala bilməz.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Göstərilmə tarixi qala bilməz.")]
        public DateTime ShowDate { get; set; }
        public string Date { get; set; }
        public bool Status { get; set; }
    }
}