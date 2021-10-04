using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Wishlist.Models;
using ILoveBaku.Application.CQRS.Wishlist.Queries.GetWishlist;
using ILoveBaku.Application.CQRS.Wishlist.Commands.AddWishlist;

namespace ILoveBaku.API.Controllers
{
    [ApiController]
    [Route("api/wishlist")]
    public class WishlistController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<WishlistVM>>> Get(int take = 10, int page = 1)
        {
            return await Mediator.Send(new GetWishlistQuery(take, page));
        }

        [HttpPost("{productStockId}")]
        public async Task<ActionResult<ApiResult<bool?>>> Process(int productStockId)
        {
            return await Mediator.Send(new ProcessWishlistCommand(productStockId));
        }
    }
}
