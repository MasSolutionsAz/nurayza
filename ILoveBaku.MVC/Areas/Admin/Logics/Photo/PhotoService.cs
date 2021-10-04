using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Product.Commands.AddProductFile;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Photo
{
    public class PhotoService : BaseService, IPhotoService
    {
        public PhotoService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        public async Task<object> Process(string path, List<PhotoModel> upload, List<string> delete, int productId,string url)
        {
            var response = new List<PhotoModel>();
            foreach (var file in upload)
            {
                ProductFileDto fileDto = new ProductFileDto
                {
                    ContentType = file.File.ContentType,
                    IsMain = file.IsMain,
                    Length = file.File.Length,
                    Name = file.File.FileName,
                    Path = url
                };

                var data = await API.PostAsync<ProductFileDto, ApiResult<PhotoModel>>($"products/{productId}/files", fileDto);
                if (data != null && !data.Succeeded)
                    return new { errors = data.ErrorList, status = 400 };

                if (data == null)
                    return new { errors = new Dictionary<string, string> { { "xəta", "Gözlənilməz xəta baş verdi." } }, status = 400 };

                if (data != null && data.Succeeded)
                {

                    string photoPath = path+ "/" + data.Response.Name;
                    using (FileStream stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await file.File.CopyToAsync(stream);
                    }
                    response.Add(new PhotoModel { IsMain = data.Response.IsMain, Name = data.Response.Name });
                }
            }

            foreach (var file in delete)
            {
                var data = await API.DeleteAsync<ApiResult<string>>($"products/{productId}/files/?name={file}");
                if (data != null && !data.Succeeded)
                    return new { errors = data.ErrorList, status = 400 };

                if (data == null)
                    return new { errors = new Dictionary<string, string> { { "xəta", "Gözlənilməz xəta baş verdi." } }, status = 400 };

                if (data.Succeeded)
                {
                    string photoPath = path + "/" + data.Response;
                    if (System.IO.File.Exists(photoPath))
                    {
                        File.Delete(photoPath);
                    }
                }
            }
            return new
            {
                status = 200,
                data = response
            };
        }
    }
}
