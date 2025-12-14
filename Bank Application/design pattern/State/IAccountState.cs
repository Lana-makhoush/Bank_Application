using Bank_Application.Models;

namespace Bank_Application.DesignPatterns.State
{
    public interface IAccountState
    {
        void HandleAccount(ClientAccount clientAccount);
        void HandleSubAccount(SubAccount subAccount);
    }
}
