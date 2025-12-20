//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Bank_Application.Models;
//using Bank_Application.Repositories;

//public class FeatureDeductionBackgroundService : BackgroundService
//{
//    private readonly IServiceScopeFactory _scopeFactory;

//    public FeatureDeductionBackgroundService(IServiceScopeFactory scopeFactory)
//    {
//        _scopeFactory = scopeFactory;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            // شهرياً: TimeSpan.FromDays(30)
//            // للتجربة: دقيقة
//            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

//            using var scope = _scopeFactory.CreateScope();

//            var clientRepo = scope.ServiceProvider.GetRequiredService<IClientAccountRepository>();
//            var subRepo = scope.ServiceProvider.GetRequiredService<ISubAccountRepository>();
//            var featureRepo = scope.ServiceProvider.GetRequiredService<IFeatureRepository>();

//            // --- الحسابات الرئيسية ---
//            var clientAccounts = await clientRepo.GetAllAsync();
//            foreach (var ca in clientAccounts)
//            {
//                if (ca.AccountTypeId == null) continue;

//                var features = await featureRepo.GetFeaturesByAccountType(ca.AccountTypeId.Value);
//                decimal totalDeduction = 0;

//                foreach (var f in features)
//                {
//                    if (decimal.TryParse(f.Cost, out decimal cost))
//                        totalDeduction += cost;
//                }

//                if (totalDeduction > 0)
//                {
//                    ca.Balance -= totalDeduction;
//                    await clientRepo.UpdateBalanceAsync(ca.ClientId, ca.AccountId, ca.Balance);
//                }
//            }

//            var subAccounts = await subRepo.GetAllAsync();
//            foreach (var sa in subAccounts)
//            {
//                if (sa.SubAccountTypeId == null) continue;

//                var features = await featureRepo.GetFeaturesByAccountType(sa.SubAccountTypeId.Value);
//                decimal totalDeduction = 0;

//                foreach (var f in features)
//                {
//                    if (decimal.TryParse(f.Cost, out decimal cost))
//                        totalDeduction += cost;
//                }

//                if (totalDeduction > 0)
//                {
//                    sa.Balance -= totalDeduction;
//                    await subRepo.UpdateBalanceAsync(sa.SubAccountId, sa.Balance);
//                }
//            }
//        }
//    }
//}
