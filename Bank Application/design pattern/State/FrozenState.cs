using Bank_Application.Models;

namespace Bank_Application.DesignPatterns.State
{
    public class FrozenState : IAccountState
    {
        public void HandleAccount(ClientAccount clientAccount)
        {
            if (clientAccount.Account != null)
            {
                clientAccount.Account.AccountStatusId = 2; 
            }
        }

        public void HandleSubAccount(SubAccount subAccount)
        {
            subAccount.SubAccountStatusId = 2; 
        }
    }
}
