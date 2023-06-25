using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("Api/accounts")]
    public class AccountController : GeneralController<Account>
    {
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository) : base(repository)
        {
            _repository = repository;
        }
    }

 
}
