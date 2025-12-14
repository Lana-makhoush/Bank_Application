using Bank_Application.Models;

namespace Bank_Application.DesignPatterns.State
{
    public class AccountContext
    {
        private IAccountState _state;
        private readonly ClientAccount? _clientAccount;
        private readonly SubAccount? _subAccount;

        public AccountContext(ClientAccount clientAccount)
        {
            _clientAccount = clientAccount;
            _state = new ActiveState();
        }

        public AccountContext(SubAccount subAccount)
        {
            _subAccount = subAccount;
            _state = new ActiveState();
        }

        public void SetState(IAccountState state)
        {
            _state = state;
        }

        public void ApplyState()
        {
            if (_clientAccount != null)
                _state.HandleAccount(_clientAccount);

            if (_subAccount != null)
                _state.HandleSubAccount(_subAccount);
        }

        public void Activate()
        {
            SetState(new ActiveState());
            ApplyState();
        }

        public void Close()
        {
            SetState(new ClosedState());
            ApplyState();
        }

        public void Suspend()
        {
            SetState(new SuspendedState());
            ApplyState();
        }

        public void Freeze()
        {
            SetState(new FrozenState());
            ApplyState();
        }
    }
}
