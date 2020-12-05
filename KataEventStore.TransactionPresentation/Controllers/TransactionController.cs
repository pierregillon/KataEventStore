using System.Collections.Generic;
using System.Threading.Tasks;
using KataEventStore.TransactionPresentation.Projections;
using KataEventStore.TransactionPresentation.Queries.GetTransactionRenamedStatistics;
using KataEventStore.TransactionPresentation.Queries.ListActiveTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KataEventStore.TransactionPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IEnumerable<TransactionListItem>> ListActiveTransactions()
            => await _mediator.Send(new ListActiveTransactionListItemQuery());

        [HttpGet("transactionRenamedStatistics")]
        public async Task<IEnumerable<TransactionRenamedStatistic>> GetTransactionRenamedStatistics()
            => await _mediator.Send(new GetTransactionRenamedStatisticsQuery());
    }
}