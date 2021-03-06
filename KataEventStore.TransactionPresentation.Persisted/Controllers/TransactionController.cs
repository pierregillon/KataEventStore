﻿using System.Collections.Generic;
using System.Threading.Tasks;
using KataEventStore.TransactionPresentation.Persisted.Queries.ListActiveTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KataEventStore.TransactionPresentation.Persisted.Controllers
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
    }
}