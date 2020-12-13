using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAsync;
using KataEventStore.TransactionPresentation.Persisted.Queries.ListActiveTransactions;
using Newtonsoft.Json;

namespace KataEventStore.TransactionPresentation.Persisted.Projections
{
    public class FileTransactionRepository : ITransactionRepository
    {
        private const string FILE_NAME = "transactions.json";

        private string FilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FILE_NAME);

        public async Task AddAsync(TransactionListItem item)
        {
            var transactions = await ReadAllTransactions();
            transactions.Add(item);
            await Save(transactions);
        }

        public Task Save(IEnumerable<TransactionListItem> transactions)
        {
            if (!File.Exists(FilePath)) {
                File.Create(FilePath);
            }
            return JsonConvert
                .SerializeObject(transactions)
                .Pipe(x => File.WriteAllTextAsync(FilePath, x));
        }

        async Task<IReadOnlyCollection<TransactionListItem>> ITransactionRepository.GetTransactions()
            => await ReadAllTransactions();

        private async Task<List<TransactionListItem>> ReadAllTransactions()
        {
            if (!File.Exists(FilePath)) {
                return new List<TransactionListItem>();
            }
            return await File
                .ReadAllTextAsync(FilePath)
                .PipeAsync(JsonConvert.DeserializeObject<List<TransactionListItem>>)
                .PipeAsync(x => x ?? new List<TransactionListItem>());
        }
    }
}