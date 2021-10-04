using System;

namespace ILoveBaku.Application.CQRS.News.Models
{
    public class NewsLangVm
    {
        public int Id { get; set; }
        public string  LangName { get; set; }
        public string  Title { get; set; }
        public string TitleUrl { get; set; }
        public string Description { get; set; }
        public string ContentHtml { get; set; }
        public int ViewCount { get; set; }
        public int Status { get; set; }
    }
}