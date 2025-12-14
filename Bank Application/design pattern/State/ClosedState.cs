using Bank_Application.DesignPatterns.State;
using Bank_Application.Models;

public class ClosedState : IAccountState
{
    public void HandleAccount(ClientAccount clientAccount)
    {
        if (clientAccount.Account != null)
            clientAccount.Account.AccountStatusId = 4;

        clientAccount.Balance = 0;
    }

    public void HandleSubAccount(SubAccount subAccount)
    {
        subAccount.SubAccountStatusId = 4;
        subAccount.Balance = 0;
    }
}
