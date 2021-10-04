using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Branches.Queries.GetBranches;
using ILoveBaku.Application.CQRS.Branches.Queries.GetSuppliers;
using ILoveBaku.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/branches")]
    [ApiController]
    public class BranchController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<BranchesDto>>>> GetBranches()
        {
            return await Mediator.Send(new GetBranchesQuery());
        }

        [HttpGet("suppliers")]
        public async Task<ActionResult<ApiResult<List<SupplierDto>>>> GetSuppliers()
        {
            return await Mediator.Send(new GetSuppliersQuery());
        }

    }
}
