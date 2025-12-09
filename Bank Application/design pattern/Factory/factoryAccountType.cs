using Bank_Application.DTOs;
using Bank_Application.Models;

namespace Bank_Application.Factories
{
    public class factoryAccountType
    {
        public AccountType CreateAccountType(AccountTypeDto dto)
        {
            return new AccountType
            {
                TypeName = dto.TypeName,
                AnnualInterestRate = dto.AnnualInterestRate,
                DailyWithdrawalLimit = dto.DailyWithdrawalLimit,
                MonthlyFee = dto.MonthlyFee,
                TermsAndConditions = dto.TermsAndConditions
            };
        }
        public void UpdateAccountType(AccountType existing, EditAccountTypeDto dto)
        {
            if (!string.IsNullOrEmpty(dto.TypeName))
                existing.TypeName = dto.TypeName;

            if (!string.IsNullOrEmpty(dto.AnnualInterestRate))
                existing.AnnualInterestRate = decimal.Parse(dto.AnnualInterestRate);

            if (!string.IsNullOrEmpty(dto.DailyWithdrawalLimit))
                existing.DailyWithdrawalLimit = decimal.Parse(dto.DailyWithdrawalLimit);

            if (!string.IsNullOrEmpty(dto.MonthlyFee))
                existing.MonthlyFee = decimal.Parse(dto.MonthlyFee);

            if (!string.IsNullOrEmpty(dto.TermsAndConditions))
                existing.TermsAndConditions = dto.TermsAndConditions;
        }
    }
}

