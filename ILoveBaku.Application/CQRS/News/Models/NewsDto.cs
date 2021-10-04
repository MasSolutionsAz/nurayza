using System;

namespace ILoveBaku.Application.CQRS.News.Models
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ShowDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Image { get; set; }
        public byte Status { get; set; }
    }
}