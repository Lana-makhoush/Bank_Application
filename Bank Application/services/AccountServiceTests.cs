using Bank_Application.Dtos;
using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Bank_Application.Services;
using Moq;
using Xunit;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _repoMock;
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        _repoMock = new Mock<IAccountRepository>();
        _service = new AccountService(_repoMock.Object);
    }

    [Fact]
    public async Task UpdateAccount_AccountNotFound_ReturnsNull()
    {
        _repoMock.Setup(r => r.UpdateAccount(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()))
                 .ReturnsAsync((ClientAccount?)null);

        var result = await _service.UpdateAccount(1, 2, new UpdateAccountDto { Balance = 100 });

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAccount_ValidData_ReturnsUpdatedAccount()
    {
        var updatedAccount = new ClientAccount
        {
            Id = 1,
            Balance = 500,
            Client = new Client { FirstName = "Ali", MiddleName = "A", LastName = "Hassan" },
            Account = new Account
            {
                AccountType = new AccountType { TypeName = "Saving" }
            }
        };

        _repoMock.Setup(r => r.UpdateAccount(1, 2, 500))
                 .ReturnsAsync(updatedAccount);

        var dto = new UpdateAccountDto { Balance = 500 };

        var result = await _service.UpdateAccount(1, 2, dto);

        Assert.NotNull(result);
        Assert.Equal(500, result!.Balance);
    }
}
