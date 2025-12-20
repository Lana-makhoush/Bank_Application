using Bank_Application.Models;

namespace Bank_Application.design_pattern.Observe
{
    public interface ITransactionSubject
    {
        void Register(ITransactionObserver observer);
        Task NotifyDepositAsync(TransactionLog transaction);
        Task NotifyWithdrawalAsync(TransactionLog transaction);
        Task NotifyManagerAsync(TransactionLog transaction);
        Task NotifyTransferAsync(TransactionLog transaction);
        Task NotifyTransferToAsync(int? ClientId, decimal Amount);
    }

}
