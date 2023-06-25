using API.Contracts;
using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/universities")]
    public class UniversityController : GeneralController<University>
    {
        private readonly IUniversityRepository _repository;

        public UniversityController(IUniversityRepository repository) : base (repository)
        {
            _repository = repository;
        }

    }
}
