using Bank_Application.DTOs;

namespace Bank_Application.services
{
  
    public interface IAuthService
    {
        Task<(bool Success, string? Message)> RegisterClientAsync(ClientRegisterDto dto);
        Task<(bool Success, string? Message)> VerifyOtpAsync(VerifyOtpDto dto);
        Task<(bool Success, AuthResponseDto? Auth, string? Message)> LoginAsync(LoginDto dto);
    }
}
