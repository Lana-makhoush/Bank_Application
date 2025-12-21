using Bank_Application.DTOs;
using Bank_Application.Models;
using Bank_Application.Repositories;
using Bank_Application.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.Tests
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _mockRepo;
        private readonly ClientService _service;

        public ClientServiceTests()
        {
            _mockRepo = new Mock<IClientRepository>();
            _service = new ClientService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateClient_InvalidIncome_ReturnsNull()
        {
            var dto = new ClientDto { MonthlyIncome = "-10" };

            var result = await _service.CreateClient(dto);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateClient_ValidClient_ReturnsClient()
        {
            var dto = new ClientDto { MonthlyIncome = "1000", FirstName = "Test", LastName = "User", Username = "testuser", Password = "pass", Email = "a@b.com" };
            var client = new Client { ClientId = 1 };

            _mockRepo.Setup(r => r.AddClient(It.IsAny<Client>())).ReturnsAsync(client);

            var result = await _service.CreateClient(dto);

            Assert.NotNull(result);
            Assert.Equal(client, result);
        }
    }

}
