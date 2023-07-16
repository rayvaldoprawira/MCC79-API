using API.DTOs.Accounts;
using API.Utilities.Enums;
using Client.Repositories;

namespace Client.Contracts
{
    public interface IAccountRepository : IRepository<RegisterDto, string>
    {
        public Task<ResponseHandler<AccountRepository>> Register(RegisterDto entity);
        public Task<ResponseHandler<string>> Login(LoginDto entity);
    }
}
