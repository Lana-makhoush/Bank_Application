using Bank_Application.Models;

namespace Bank_Application.DesignPatterns.State
{
    public class SuspendedState : IAccountState
    {
        public void HandleAccount(ClientAccount clientAccount)
        {
            if (clientAccount.Account != null)
            {
                clientAccount.Account.AccountStatusId = 3; 
            }
        }

        public void HandleSubAccount(SubAccount subAccount)
        {
            subAccount.SubAccountStatusId = 3; 
        }
    }
}
