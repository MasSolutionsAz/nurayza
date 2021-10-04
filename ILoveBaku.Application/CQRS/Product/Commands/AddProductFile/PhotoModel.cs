using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProductFile
{

    public class PhotoModel
    {
        public bool IsMain { get; set; }
        public IFormFile File { get; set; }
        public string Name { get; set; }
    }
}
