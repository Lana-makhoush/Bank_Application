using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Services;
using Bank_Application.Services.Facade;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.Tests.Services
{
    public class AccountFacadeServiceTests
    {
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly AccountFacadeService _facadeService;

        public AccountFacadeServiceTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _facadeService = new AccountFacadeService(_mockAccountService.Object);
        }

        [Fact]
        public async Task CreateAccountForClient_ClientNotExists_ReturnsNull()
        {
            _mockAccountService.Setup(s => s.ClientExists(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _facadeService.CreateAccountForClient(1, 1, 1, new AccountDto());

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAccountForClient_AccountTypeNotExists_ReturnsError()
        {
            _mockAccountService.Setup(s => s.ClientExists(It.IsAny<int>())).ReturnsAsync(true);
            _mockAccountService.Setup(s => s.AccountTypeExists(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _facadeService.CreateAccountForClient(1, 99, 1, new AccountDto());

            Assert.Equal("ACCOUNT_TYPE_NOT_FOUND", result);
        }

        [Fact]
        public async Task CreateAccountForClient_ClientAccountExists_ReturnsDuplicate()
        {
            _mockAccountService.Setup(s => s.ClientExists(It.IsAny<int>())).ReturnsAsync(true);
            _mockAccountService.Setup(s => s.AccountTypeExists(It.IsAny<int>())).ReturnsAsync(true);
            _mockAccountService.Setup(s => s.ClientAccountExistsWithSameData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<AccountDto>()))
                .ReturnsAsync(true);

            var result = await _facadeService.CreateAccountForClient(1, 1, 1, new AccountDto());

            Assert.Equal("DUPLICATE_ACCOUNT_WITH_SAME_DATA", result);
        }

        [Fact]
        public async Task CreateAccountForClient_Success_ReturnsClientAccount()
        {
            var dto = new AccountDto();
            var account = new Account { AccountId = 1 };
            var clientAccount = new ClientAccount { Id = 1 };

            _mockAccountService.Setup(s => s.ClientExists(It.IsAny<int>())).ReturnsAsync(true);
            _mockAccountService.Setup(s => s.AccountTypeExists(It.IsAny<int>())).ReturnsAsync(true);
            _mockAccountService.Setup(s => s.ClientAccountExistsWithSameData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<AccountDto>()))
                .ReturnsAsync(false);
            _mockAccountService.Setup(s => s.CreateAccount(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(account);
            _mockAccountService.Setup(s => s.CreateClientAccount(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<AccountDto>()))
                .ReturnsAsync(clientAccount);
            _mockAccountService.Setup(s => s.GetFeaturesByAccountType(It.IsAny<int>())).ReturnsAsync(new List<Feature>());
            _mockAccountService.Setup(s => s.DeductFeaturesFromAccount(It.IsAny<ClientAccount>(), It.IsAny<List<Feature>>()))
                .ReturnsAsync(new List<object>());

            var result = await _facadeService.CreateAccountForClient(1, 1, 1, dto);

            Assert.Equal(clientAccount, result);
        }
    }
}