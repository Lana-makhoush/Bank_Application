using Bank_Application.DTOs;

namespace Bank_Application.Services
{
    public interface ITransactionService
    {
        Task<ServiceResult> DepositAsync(DepositDto dto);
        Task<ServiceResult> WithdrawalAsync(DepositDto dto);
        Task<ServiceResult> TransferAsync(TransferDto dto);
    }
}
