using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;

namespace LaptopRentalManagement.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDetailResponse> CustomerRegister(AccountRegisterRequest request);
        Task<AccountDetailResponse> AdminCreateAccount(AccountRegisterRequest request);
        Task<AccountDetailResponse> Login(string email, string password);
        Task<AccountDetailResponse> GetById(int id);
        Task<IEnumerable<AccountDetailResponse>> GetAll();
        Task<AccountDetailResponse> Update(AccountUpdateRequest request);
        Task<bool> Delete(int id);
    }
}
