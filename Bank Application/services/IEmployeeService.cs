using Bank_Application.DTOs;

namespace Bank_Application.Services
{
    public interface IEmployeeService
    {
        Task<ServiceResult> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<LoginEmployeeResult> LoginEmployeeAsync(LoginEmployeeDto dto);
        Task<ServiceResult> ChangePasswordAsync(int employeeId, ChangePasswordDto dto);
    }
}
