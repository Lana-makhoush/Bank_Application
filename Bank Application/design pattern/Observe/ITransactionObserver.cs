using Bank_Application.Models;

namespace Bank_Application.design_pattern.Observe
{
    public interface ITransactionObserver
    {
        Task OnTransactionCompletedDepositAsync(TransactionLog transaction);
        Task OnTransactionCompletedWithdrawalAsync(TransactionLog transaction);
        Task OnTransactionManagerAsync(TransactionLog transaction);
        Task OnTransactionTransferAsync(TransactionLog transaction);
        Task OnTransactionTransferToAsync(int? ClientId, decimal Amount);
    }

}
