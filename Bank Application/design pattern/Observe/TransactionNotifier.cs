using Bank_Application.Models;

namespace Bank_Application.design_pattern.Observe
{
    public class TransactionNotifier : ITransactionSubject
    {
        private readonly List<ITransactionObserver> _observers = new();

        public void Register(ITransactionObserver observer)
        {
            _observers.Add(observer);
        }

        public async Task NotifyDepositAsync(TransactionLog transaction)
        {
            foreach (var observer in _observers)
            {
                await observer.OnTransactionCompletedDepositAsync(transaction);
            }
        }
        public async Task NotifyWithdrawalAsync(TransactionLog transaction)
        {
            foreach (var observer in _observers)
            {
                await observer.OnTransactionCompletedWithdrawalAsync(transaction);
            }
        }
        public async Task NotifyManagerAsync(TransactionLog transaction)
        {
            foreach (var observer in _observers)
            {
                await observer.OnTransactionManagerAsync(transaction);
            }
        }
        public async Task NotifyTransferAsync(TransactionLog transaction)
        {
            foreach (var observer in _observers)
            {
                await observer.OnTransactionTransferAsync(transaction);
            }
        }

        public async Task NotifyTransferToAsync(int? ClientId, decimal Amount)
        {
            foreach (var observer in _observers)
            {
                await observer.OnTransactionTransferToAsync( ClientId,  Amount);
            }
        }
    }

}
