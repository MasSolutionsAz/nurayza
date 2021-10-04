using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Content.Models
{
    public class ContentDto
    {
        public string Title { get; set; }

        public string Content { get; set; }
        public int ContentId { get; set; }
        public int ContentLangId { get; set; }

        public string Image { get; set; }
        public string ImageName { get; set; }
        public string LangName { get; set; }
    }
}
