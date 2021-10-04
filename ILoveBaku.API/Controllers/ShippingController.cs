using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacketDetail;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPacket;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPackets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/shippings")]
    [ApiController]
    public class ShippingController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<ProductCashOutShippingPacketDto>>>> GetProductCashOutShippingPackets()
        {
            return await Mediator.Send(new GetProductCashOutShippingPacketsQuery());
        }

        [HttpGet("{productCashOutId}")]
        public async Task<ActionResult<ApiResult<ProductsCashOutShippingsPacketsDetailsDto>>> GetProductCashOutShippingPacket(int productCashOutId)
        {
            return await Mediator.Send(new GetProductCashOutShippingPacketQuery() { ProductCashOutId = productCashOutId });
        }
        [HttpPost("packets")]
        public async Task<ActionResult<ApiResult<int?>>> CreatePacket(string Name)
        {
            return await Mediator.Send(new CreateShippingPacketCommand { Name = Name });
        }

        [HttpPost("packets/{productCashOutId}/details")]
        public async Task<ActionResult<ApiResult<int?>>> CreatePacketDetail(int packetId,int productCashOutId)
        {
            return await Mediator.Send(new CreateShippingPacketDetailCommand { PacketId = packetId, ProductCashOutId = productCashOutId });
        }
    }
}