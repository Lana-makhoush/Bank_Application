using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Application.Tests
{
    using Bank_Application.DTOs;
    using Bank_Application.Models;
    using Bank_Application.Repositories;
    using Bank_Application.services;
    using Bank_Application.Services;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using Xunit;

    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _repoMock;
        private readonly Mock<IEmailService> _emailMock;
        private readonly Mock<IJwtService> _jwtMock;
        private readonly IConfiguration _config;

        public EmployeeServiceTests()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _emailMock = new Mock<IEmailService>();
            _jwtMock = new Mock<IJwtService>();

            var configMock = new ConfigurationBuilder().AddInMemoryCollection().Build();
            _config = configMock;
        }

       
        [Fact]
        public async Task CreateEmployeeAsync_ShouldFail_WhenEmailExists()
        {
            _repoMock.Setup(r => r.EmailExistsAsync("test@mail.com"))
                     .ReturnsAsync(true);

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new CreateEmployeeDto
            {
                Email = "test@mail.com",
                Username = "user1",
                Role = "Admin"
            };

            var result = await service.CreateEmployeeAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("الإيميل مستخدم مسبقاً", result.Message);
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldFail_WhenUsernameExists()
        {
            _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
                     .ReturnsAsync(false);

            _repoMock.Setup(r => r.UsernameExistsAsync("user1"))
                     .ReturnsAsync(true);

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new CreateEmployeeDto
            {
                Email = "test@mail.com",
                Username = "user1",
                Role = "Admin"
            };

            var result = await service.CreateEmployeeAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("اسم المستخدم مستخدم مسبقاً", result.Message);
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldCreateEmployee_AndSendEmail()
        {
            _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
                     .ReturnsAsync(false);

            _repoMock.Setup(r => r.UsernameExistsAsync(It.IsAny<string>()))
                     .ReturnsAsync(false);

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new CreateEmployeeDto
            {
                Email = "test@mail.com",
                Username = "user1",
                Role = "Teller"
            };

            var result = await service.CreateEmployeeAsync(dto);

            Assert.True(result.Success);
            Assert.Contains("تم إنشاء حساب الموظف", result.Message);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);

            _emailMock.Verify(e => e.SendEmailAsync(
                dto.Email,
                "بيانات الدخول",
                It.Is<string>(msg => msg.Contains("Username: user1"))
            ), Times.Once);
        }

      
        [Fact]
        public async Task LoginEmployeeAsync_ShouldFail_WhenUserNotFound()
        {
            _repoMock.Setup(r => r.GetByUsernameAsync("user1"))
                     .ReturnsAsync((Employee)null);

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new LoginEmployeeDto { Username = "user1", Password = "123" };

            var result = await service.LoginEmployeeAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("اسم المستخدم أو كلمة المرور غير صحيحة", result.Message);
        }

        [Fact]
        public async Task LoginEmployeeAsync_ShouldFail_WhenAccountInactive()
        {
            var emp = new Employee
            {
                Username = "user1",
                Password = BCrypt.Net.BCrypt.HashPassword("123"),
                IsActive = false
            };

            _repoMock.Setup(r => r.GetByUsernameAsync("user1"))
                     .ReturnsAsync(emp);

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new LoginEmployeeDto { Username = "user1", Password = "123" };

            var result = await service.LoginEmployeeAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("الحساب غير مفعل", result.Message);
        }

        [Fact]
        public async Task LoginEmployeeAsync_ShouldFail_WhenPasswordIncorrect()
        {
            var emp = new Employee
            {
                Username = "user1",
                Password = BCrypt.Net.BCrypt.HashPassword("correct"),
                IsActive = true
            };

            _repoMock.Setup(r => r.GetByUsernameAsync("user1"))
                     .ReturnsAsync(emp);

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new LoginEmployeeDto { Username = "user1", Password = "wrong" };

            var result = await service.LoginEmployeeAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("اسم المستخدم أو كلمة المرور غير صحيحة", result.Message);
        }

        [Fact]
        public async Task LoginEmployeeAsync_ShouldReturnToken_WhenSuccess()
        {
            var emp = new Employee
            {
                EmployeeId = 5,
                Username = "user1",
                Password = BCrypt.Net.BCrypt.HashPassword("123"),
                IsActive = true,
                Role = "Admin",
                MustChangePassword = false
            };

            _repoMock.Setup(r => r.GetByUsernameAsync("user1"))
                     .ReturnsAsync(emp);

            _jwtMock.Setup(j => j.GenerateToken(emp.EmployeeId, emp.Role, out It.Ref<DateTime>.IsAny))
                    .Returns("FAKE_TOKEN");

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new LoginEmployeeDto { Username = "user1", Password = "123" };

            var result = await service.LoginEmployeeAsync(dto);

            Assert.True(result.Success);
            Assert.Equal("FAKE_TOKEN", result.Token);
        }

       
        [Fact]
        public async Task ChangePasswordAsync_ShouldFail_WhenEmployeeNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((Employee)null);

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new ChangePasswordDto { OldPassword = "123", NewPassword = "456" };

            var result = await service.ChangePasswordAsync(1, dto);

            Assert.False(result.Success);
            Assert.Equal("الموظف غير موجود", result.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldFail_WhenOldPasswordIncorrect()
        {
            var emp = new Employee
            {
                EmployeeId = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("correct")
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(emp);

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new ChangePasswordDto { OldPassword = "wrong", NewPassword = "456" };

            var result = await service.ChangePasswordAsync(1, dto);

            Assert.False(result.Success);
            Assert.Equal("كلمة المرور القديمة غير صحيحة", result.Message);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldUpdatePassword_WhenSuccess()
        {
            var emp = new Employee
            {
                EmployeeId = 1,
                Password = BCrypt.Net.BCrypt.HashPassword("123"),
                MustChangePassword = true
            };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(emp);

            var service = new EmployeeService(_repoMock.Object, _emailMock.Object, _config, _jwtMock.Object);

            var dto = new ChangePasswordDto { OldPassword = "123", NewPassword = "456" };

            var result = await service.ChangePasswordAsync(1, dto);

            Assert.True(result.Success);
            Assert.Equal("تم تغيير كلمة المرور بنجاح", result.Message);

            _repoMock.Verify(r => r.UpdateAsync(emp), Times.Once);
            Assert.False(emp.MustChangePassword);
        }
    }

}
