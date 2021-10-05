using System.Collections.Generic;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Branches.Queries.GetBranches;
using ILoveBaku.Application.CQRS.Contact.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Controllers
{
    public class ContactController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var branches = await API.GetAsync<ApiResult<List<BranchesDto>>>("branches");
            if (branches != null)
            {
                ContactVM vm = new ContactVM
                {
                    Branches = branches.Succeeded ? branches.Response : new List<BranchesDto>()
                };
                return View(vm);
            }

            return View();

        }

        [HttpPost]
        public async Task<JsonResult> Send(ContactVM model)
        {
            ApiResult<int> response = await API.PostAsync<ContactVM, ApiResult<int>>("contact/send", model);

            if (!response.Succeeded)
                return Json(new
                {
                    status = response.Response,
                    error = response.ErrorDetail.ErrorMessage
                });

            return Json(new
            {
                status = 200
            });
        }
    }
}