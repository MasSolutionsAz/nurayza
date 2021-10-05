using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Branches.Queries.GetBranches;
using ILoveBaku.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Components
{
    public class FooterBranchViewComponent : ViewComponent
    {
        private readonly ApiRequestService API;

        public FooterBranchViewComponent(ApiRequestService apiRequestService, CultureService cultureService)
        {
            API = apiRequestService;
            API.Configure(options =>
            {
                options.AddHeader("culture", cultureService.CurrentCulture);
            });
        }

        public IViewComponentResult Invoke()
        {
            var branches = API.GetAsync<ApiResult<List<BranchesDto>>>("branches").Result;

            if (branches != null && branches.Succeeded)
            {
                var onlineBranch = branches.Response.Where(c => c.IsOnlineBranch).FirstOrDefault();
                if (onlineBranch != null)
                    return View(onlineBranch);
            }

            return View(new BranchesDto());
        }

    }
}
