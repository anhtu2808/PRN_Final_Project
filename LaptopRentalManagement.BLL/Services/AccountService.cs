using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.DAL.Entities;
using LaptopRentalManagement.DAL.Interfaces;

namespace LaptopRentalManagement.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<AccountDetailResponse> CustomerRegister(AccountRegisterRequest request)
        {
            request.Role = "Customer";
            var acc = _accountRepository.CreateAsync(_mapper.Map<Account>(request));
            return _mapper.Map<AccountDetailResponse>(await acc);
        }

        public async Task<AccountDetailResponse> AdminCreateAccount(AccountRegisterRequest request)
        {
            var acc = _accountRepository.CreateAsync(_mapper.Map<Account>(request));
            return _mapper.Map<AccountDetailResponse>(await acc);
        }

        public async Task<AccountDetailResponse> Login(string email, string password)
        {
            var acc = await _accountRepository.Login(email, password);
            return _mapper.Map<AccountDetailResponse>(acc);
        }

        public async Task<AccountDetailResponse> GetById(int id)
        {
            var acc = await _accountRepository.GetByIdAsync(id);
            return _mapper.Map<AccountDetailResponse>(acc);
        }

        public async Task<IEnumerable<AccountResponse>> GetAll()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AccountResponse>>(accounts);
        }

        public async Task<AccountDetailResponse> Update(AccountUpdateRequest request)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId);
            if (account == null)
                return null;
            account.Name = request.Name;
            account.Email = request.Email;
            account.PasswordHash = request.PasswordHash;
            account.Role = request.Role ?? account.Role;
            var updatedAccount = await _accountRepository.UpdateAsync(account);
            return _mapper.Map<AccountDetailResponse>(updatedAccount);
        }

        public async Task<bool> Delete(int id)
        {
            return await _accountRepository.DeleteAsync(id);
        }
    }
}
