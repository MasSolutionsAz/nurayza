using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Commands.CreateShippingPacket;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.ProductCashOut.Commands.UpdateProductCashOut
{
    public class UpdateProductCashOutCommand:BaseRequest<ApiResult<int?>>
    {

    }
}
