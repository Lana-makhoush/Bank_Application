using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Bank_Application.Models
{
    public class Account
    {
//
        [Key] public int? AccountId { get; set; }
        public ICollection<ClientAccount>? ClientAccounts { get; set; }
        public int? AccountTypeId { get; set; }
        [MaxLength(30)]
        public AccountType? AccountType { get; set; }
        public int? AccountStatusId { get; set; }
        public AccountStatus? AccountStatus { get; set; }
        public int? ParentAccountId { get; set; }
        public Account? ParentAccount { get; set; }
        public ICollection<Account>? SubAccounts { get; set; }
        public ICollection<ScheduledTransaction>? ScheduledTransactions { get; set; }
        public ICollection<TransactionLog>? ReceivedTransactions { get; set; }

    }
}
        