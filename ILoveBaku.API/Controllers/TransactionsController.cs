using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Transactions.Commands.AddProductTransaction;
using ILoveBaku.Application.CQRS.Transactions.Commands.AddProductTransactionDetails;
using ILoveBaku.Application.CQRS.Transactions.Commands.DeleteTransactionDetail;
using ILoveBaku.Application.CQRS.Transactions.Commands.UpdateProductTransaction;
using ILoveBaku.Application.CQRS.Transactions.Models;
using ILoveBaku.Application.CQRS.Transactions.Queries.GetProductStockTransactionDetails;
using ILoveBaku.Application.CQRS.Transactions.Queries.GetProductStockTransactions;
using ILoveBaku.Application.CQRS.Transactions.Queries.GetTransactionById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<ProductTransactionDto>>>> GetTransactions([FromQuery]ProductTransactionFilter model)
        {
            return await Mediator.Send(new GetProductStockTransactionsQuery() { Model = model });
        }

        [HttpGet("{transactionId}/details")]
        public async Task<ActionResult<ApiResult<List<ProductTransactionDetailsDto>>>> GetTransactionsDetails(int transactionId)
        {
            return await Mediator.Send(new GetProductStockTransactionDetailsQuery { TransactionId = transactionId });
        }
        [HttpGet("{transactionId}")]
        public async Task<ActionResult<ApiResult<ProductTransactionDto>>> GetTransactionById(int transactionId,byte transactionType)
        {
            return await Mediator.Send(new GetTransactionByIdQuery() {TransactionId = transactionId,TransactionType = transactionType });
        }
        [HttpPost]
        public async Task<ActionResult<ApiResult<int?>>> AddTransaction(ProductTransactionCreateDto model)
        {
            return await Mediator.Send(new AddProductTransactionCommand { Model = model });
        }
        [HttpPost("{transactionId}/details")]
        public async Task<ActionResult<ApiResult<int?>>> AddTransactionDetail(ProductTransactionDetailsModel model,int transactionId)
        {
            return await Mediator.Send(new AddProductTransactionDetailsCommand { Model = model, TransactionId = transactionId });
        }

        [HttpPut("{transactionId}")]
        public async Task<ActionResult<ApiResult<int?>>> UpdateTransaction(int transactionId,ProductTransactionDto model,byte? transactionStatus)
        {
            model.Id = transactionId;
            return await Mediator.Send(new UpdateProductTransactionCommand { Model = model ,TransactionStatus = transactionStatus});
        }
        [HttpDelete("{transactionId}/details/{transactionDetailId}")]
        public async Task<ActionResult<ApiResult<int?>>> DeleteTransactionDetail(int transactionId,int transactionDetailId)
        {
            return await Mediator.Send(new DeleteTransactionDetailCommand { TransactionDetailId = transactionDetailId, TransactionId = transactionId });
        }
    }
}
