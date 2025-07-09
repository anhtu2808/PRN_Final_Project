using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;

namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<BaseResponse<AccountResponse>> RegisterAsync(RegisterRequest request);
        Task<BaseResponse<ProfileResponse>> GetProfileAsync(int accountId);
        Task<BaseResponse<ProfileResponse>> UpdateProfileAsync(int accountId, UpdateProfileRequest request);
        Task<BaseResponse<bool>> ChangePasswordAsync(int accountId, ChangePasswordRequest request);
        Task<BaseResponse<PagedResponse<AccountResponse>>> GetAllAccountsAsync(int pageNumber, int pageSize);
        Task<BaseResponse<AccountResponse>> GetAccountByIdAsync(int accountId);
        Task<BaseResponse<bool>> DeactivateAccountAsync(int accountId);
        Task<BaseResponse<bool>> ActivateAccountAsync(int accountId);
        Task<BaseResponse<bool>> DeleteAccountAsync(int accountId);
    }
} 