using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.Tests
{
    using Xunit;
    using Moq;
    using Microsoft.EntityFrameworkCore;
    using Bank_Application.services;
    using Bank_Application.Data;
    using Bank_Application.DTOs;
    using Bank_Application.Models;
    using Bank_Application.Repositories;

    public class LoanServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public LoanServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateLoanAsync_ShouldFail_WhenNoAccountProvided()
        {
            using var context = new AppDbContext(_options);
            var repoMock = new Mock<IClientAccountTransactionRepository>();

            var service = new LoanService(context, repoMock.Object);

            var dto = new CreateLoanDto
            {
                AccountId = null,
                SubAccountId = null
            };

            var result = await service.CreateLoanAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("يجب تحديد حساب رئيسي أو فرعي", result.Message);
        }

        [Fact]
        public async Task CreateLoanAsync_ShouldFail_WhenBothAccountsProvided()
        {
            using var context = new AppDbContext(_options);
            var repoMock = new Mock<IClientAccountTransactionRepository>();

            var service = new LoanService(context, repoMock.Object);

            var dto = new CreateLoanDto
            {
                AccountId = 1,
                SubAccountId = 2
            };

            var result = await service.CreateLoanAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("لا يمكن ربط القرض بحسابين", result.Message);
        }

     


    }

}
