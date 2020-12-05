using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using KataEventStore.TransactionDomain.Domain.Application.CreateTransaction;
using KataEventStore.TransactionDomain.Domain.Application.DeleteTransaction;
using KataEventStore.TransactionDomain.Domain.Application.EditTransactionAmount;
using KataEventStore.TransactionDomain.Domain.Application.RenameTransaction;
using KataEventStore.TransactionDomain.Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KataEventStore.TransactionDomain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<Guid> CreateTransaction([Required] [FromBody] CreateTransactionDto model)
            => await _mediator.Send(new CreateTransactionCommand(model.Name, model.Amount));

        [HttpPut("amount")]
        public async Task EditAmount([Required] [FromBody] EditAmountDto model)
            => await _mediator.Send(new EditTransactionAmountCommand(TransactionId.From(model.TransactionId), model.Amount));

        [HttpPut("name")]
        public async Task Rename([Required] [FromBody] RenameTransactionDto model)
            => await _mediator.Send(new RenameTransactionCommand(TransactionId.From(model.TransactionId), model.Name));

        [HttpDelete("{transactionId}")]
        public async Task DeleteTransaction([Required, FromRoute] Guid transactionId)
            => await _mediator.Send(new DeleteTransactionCommand(TransactionId.From(transactionId)));
    }

    public class RenameTransactionDto
    {
        public Guid TransactionId { get; set; }
        public string Name { get; set; }
    }

    public class EditAmountDto
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateTransactionDto
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}