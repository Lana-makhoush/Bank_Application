using Bank_Application.Models;
using Microsoft.AspNetCore.SignalR;
using Bank_Application.Hubs;
using Microsoft.VisualBasic;
namespace Bank_Application.design_pattern.Observe
{
   
    public class NotificationObserver : ITransactionObserver
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationObserver(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task OnTransactionCompletedDepositAsync(TransactionLog transaction)
        {
            await _hub.Clients
                .User(transaction.ClientId.ToString())
                .SendAsync("ReceiveNotification", new
                {
                    Message = $"تم إيداع مبلغ {transaction.Amount} بنجاح",
                    Date = transaction.TransactionDate
                });
        }
        public async Task OnTransactionCompletedWithdrawalAsync(TransactionLog transaction)
        {
            await _hub.Clients
                .User(transaction.ClientId.ToString())
                .SendAsync("ReceiveNotification", new
                {
                    Message = $"تم سحب مبلغ {transaction.Amount} بنجاح",
                    Date = transaction.TransactionDate
                });
        }
        public async Task OnTransactionManagerAsync(TransactionLog transaction)
        {
            await _hub.Clients
              .User(transaction.ClientId.ToString())
              .SendAsync("ReceiveNotification", new
              {
                  Message = "بانتظار موافقة المدير ",
                  Date = transaction.TransactionDate
              });
        }
        public async Task OnTransactionTransferAsync(TransactionLog transaction)
        {
            await _hub.Clients
              .User(transaction.ClientId.ToString())
              .SendAsync("ReceiveNotification", new
              {
                  Message = $" تم تحويل مبلغ منك  {transaction.Amount} بنجاح ",
                  Date = transaction.TransactionDate
              });
        }
        public async Task OnTransactionTransferToAsync(int? ClientId ,decimal Amount)
        {
            await _hub.Clients
              .User(ClientId.ToString())
              .SendAsync("ReceiveNotification", new
              {
                  Message = $"تم تحويل مبلغ لك {Amount} بنجاح ",
                 Date= DateTime.Now
              });
        }
        

    }

}
