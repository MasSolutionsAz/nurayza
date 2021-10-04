using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Photo
{
    public interface IPhotoService
    {
        Task<object> Process(string path, List<PhotoModel> upload,List<string> delete,int productId,string url);
    }
}
