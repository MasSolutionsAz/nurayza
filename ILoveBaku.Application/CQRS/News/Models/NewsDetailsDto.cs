using System;

namespace ILoveBaku.Application.CQRS.News.Models
{
    public class NewsDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContentHTML { get; set; }
        public DateTime ShowDate { get; set; }
    }
}
