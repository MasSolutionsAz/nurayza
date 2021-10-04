using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using ILoveBaku.MVC.Areas.Admin.Logics.Category;
using ILoveBaku.MVC.Areas.Admin.Logics.CategorySpecification;
using ILoveBaku.MVC.Areas.Admin.Logics.Language;
using ILoveBaku.MVC.Areas.Admin.Logics.Menu;
using ILoveBaku.MVC.Areas.Admin.Logics.Order;
using ILoveBaku.MVC.Areas.Admin.Logics.Photo;
using ILoveBaku.MVC.Areas.Admin.Logics.Product;
using ILoveBaku.MVC.Areas.Admin.Logics.ProductStock;
using ILoveBaku.MVC.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiRequestService(this IServiceCollection services)
        {
            services.AddScoped<ApiRequestService>();
            return services;
        }

        public static IServiceCollection AddApiRequestService(this IServiceCollection services, Action<ApiRequestOptions> apiOptions)
        {
            ApiRequestOptions apiRequestOptions = new ApiRequestOptions();
            apiOptions(apiRequestOptions);
            services.AddTransient(sp => { return new ApiRequestService(apiRequestOptions); });
            return services;
        }
        public static IServiceCollection AddLogics(this IServiceCollection services)
        {
            services.AddScoped<BaseService>();
            services.AddScoped(typeof(ICategoryService<int?>), typeof(CategoryService));
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategorySpecificationService, CategorySpecificationService>();
            services.AddScoped<IProductStockService, ProductStockService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IMenuService, MenuService>();
            return services;
        }
    }
}
