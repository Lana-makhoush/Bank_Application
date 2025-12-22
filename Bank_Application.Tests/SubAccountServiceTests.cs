using Bank_Application.DTOs;
using Bank_Application.Models;
using Moq;
using Xunit;

public class SubAccountServiceTests
{
    private readonly Mock<ISubAccountRepository> _repoMock;
    private readonly SubAccountService _service;

    public SubAccountServiceTests()
    {
        _repoMock = new Mock<ISubAccountRepository>();
        _service = new SubAccountService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateSubAccountAsync_Should_Create_SubAccount_Successfully()
    {
        // Arrange
        var dto = new SubAccountCreateDto
        {
            ParentAccountId = 1,
            DailyWithdrawalLimit = 100,
            TransferLimit = 500,
            UsageAreas = "Online",
            UserPermissions = "Read",
            Balance = 1000,
            CreatedAt = DateTime.Now
        };

        _repoMock.Setup(r => r.ParentAccountExistsAsync(dto.ParentAccountId))
                 .ReturnsAsync(true);

        _repoMock.Setup(r => r.ExistsAsync(
            dto.ParentAccountId,
            dto.DailyWithdrawalLimit,
            dto.TransferLimit,
            dto.UsageAreas,
            dto.UserPermissions))
            .ReturnsAsync(false);

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<SubAccount>()))
            .ReturnsAsync((SubAccount s) =>
            {
                s.SubAccountId = 1;
                return s;
            });

        var result = await _service.CreateSubAccountAsync(dto, 1, 1);

        Assert.NotNull(result);
        Assert.Equal(dto.ParentAccountId, result.ParentAccountId);
        Assert.Equal(1, result.SubAccountStatusId);
        Assert.Equal(1, result.SubAccountTypeId);
    }
    [Fact]
    public async Task UpdateSubAccountAsync_Should_Update_SubAccount_Successfully()
    {
        var existingSubAccount = new SubAccount
        {
            SubAccountId = 1,
            ParentAccountId = 1,
            DailyWithdrawalLimit = 100,
            TransferLimit = 200,
            Balance = 500
        };

        var dto = new SubAccountUpdateDto
        {
            DailyWithdrawalLimit = "300",
            TransferLimit = "600",
            Balance = "1500"
        };

        _repoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existingSubAccount);

        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<SubAccount>()))
            .ReturnsAsync((SubAccount s) => s);

        _repoMock.Setup(r => r.LoadSubAccountStatusAsync(It.IsAny<SubAccount>()))
            .Returns(Task.CompletedTask);

        var result = await _service.UpdateSubAccountAsync(1, 2, dto, 1);

        Assert.NotNull(result);
        Assert.Equal(300, result.DailyWithdrawalLimit);
        Assert.Equal(600, result.TransferLimit);
        Assert.Equal(1500, result.Balance);
        Assert.Equal(2, result.SubAccountStatusId);
    }

}
