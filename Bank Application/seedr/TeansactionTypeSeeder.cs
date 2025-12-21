using Bank_Application.Data;
using Bank_Application.Models;
using System.Linq;

namespace Bank_Application.Seeders
{
    public static class TransactionTypeSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.TransactionTypes.Any())
            {
                context.TransactionTypes.AddRange(
                    new TransactionType { TypeName = "Deposit" },
                    new TransactionType { TypeName = "Withdraw" },
                    new TransactionType { TypeName = "Transfer" },
                    new TransactionType { TypeName = "PendingApproval" },
                    new TransactionType { TypeName = "Approved" },
                    new TransactionType { TypeName = "Rejected" }
                );

                context.SaveChanges();
            }
        }
    }
}
