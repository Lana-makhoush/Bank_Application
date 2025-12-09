using Bank_Application.Data;
using Bank_Application.Models;

namespace Bank_Application.Seeders
{
    public static class AccountStatusSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.AccountStatuses.Any())
                return;

            var statuses = new List<AccountStatus>
{
    new AccountStatus { StatusName = "نشط" },
    new AccountStatus { StatusName = "مجمد" },
    new AccountStatus { StatusName = "معلق" },
    new AccountStatus { StatusName = "مغلق" }
};


            context.AccountStatuses.AddRange(statuses);
            context.SaveChanges();
        }
    }
}
