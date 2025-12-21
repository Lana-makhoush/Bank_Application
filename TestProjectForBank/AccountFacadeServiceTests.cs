using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Services;
using Bank_Application.Services.Facade;
using Moq;
using Xunit;

public class AccountFacadeServiceTests
{
    private readonly Mock<IAccountService> _serviceMock;
    private readonly AccountFacadeService _facade;

    public AccountFacadeServiceTests()
    {
        _serviceMock = new Mock<IAccountService>();
        _facade = new AccountFacadeService(_serviceMock.Object);
    }

    [Fact]
    public async Task CreateAccountForClient_ClientNotExists_ReturnsNull()
    {
        _serviceMock.Setup(s => s.ClientExists(It.IsAny<int>()))
                    .ReturnsAsync(false);

        var result = await _facade.CreateAccountForClient(1, 1, 1, new AccountDto());

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAccountForClient_DuplicateAccountType_ReturnsError()
    {
        _serviceMock.Setup(s => s.ClientExists(1)).ReturnsAsync(true);
        _serviceMock.Setup(s => s.AccountTypeExists(1)).ReturnsAsync(true);
        _serviceMock.Setup(s => s.ClientHasAccountType(1, 1)).ReturnsAsync(true);

        var result = await _facade.CreateAccountForClient(1, 1, 1, new AccountDto());

        Assert.Equal("DUPLICATE_ACCOUNT_TYPE", result);
    }

    [Fact]
    public async Task CreateAccountForClient_ValidData_CreatesAccount()
    {
        var account = new Account { AccountId = 10 };
        var clientAccount = new ClientAccount { Id = 5, AccountId = 10 };

        _serviceMock.Setup(s => s.ClientExists(1)).ReturnsAsync(true);
        _serviceMock.Setup(s => s.AccountTypeExists(1)).ReturnsAsync(true);
        _serviceMock.Setup(s => s.ClientHasAccountType(1, 1)).ReturnsAsync(false);

        _serviceMock.Setup(s => s.CreateAccount(1, 1))
                    .ReturnsAsync(account);

        _serviceMock.Setup(s => s.CreateClientAccount(1, 10, It.IsAny<AccountDto>()))
                    .ReturnsAsync(clientAccount);

        _serviceMock.Setup(s => s.GetFeaturesByAccountType(1))
                    .ReturnsAsync(new List<Feature>());

        var result = await _facade.CreateAccountForClient(1, 1, 1, new AccountDto());

        Assert.NotNull(result);
        Assert.IsType<ClientAccount>(result);
    }
}
