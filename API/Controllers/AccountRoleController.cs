using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/accountroles")]
    public class AccountRoleController : ControllerBase
    {
        private readonly IAccountRoleRepository _repository;

        public AccountRoleController(IAccountRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var accountroles = _repository.GetAll();
            if(!accountroles.Any())
            {
                return NotFound();
            }
            return Ok(accountroles);
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var accountrole = _repository.GetByGuid(guid);
            if(accountrole is null)
            {
                return NotFound();
            }
            return Ok(accountrole);
        }

        [HttpPost]
        public IActionResult Create(AccountRole accountRole)
        {
            var isCreated = _repository.Create(accountRole);
            return Ok(isCreated);
        }

        [HttpPut]
        public IActionResult Update(AccountRole accountRole)
        {
            var isUpdate = _repository.Update(accountRole);
            if(!isUpdate)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var isDeleted = _repository.Delete(guid);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
