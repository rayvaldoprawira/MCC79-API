using API.Contracts;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/universities")]
    public class UniversityController : GeneralController<University>
    {
        private readonly IUniversityRepository _repository;

        public UniversityController(IUniversityRepository repository) : base(repository)
        {
            _repository = repository;
        }

        [HttpGet("/{name}")]
        public IActionResult GetByName(string name)
        {
            var university = _repository.GetByName(name);
            if (university is null)
            {
                return NotFound();
            }

            return Ok(university);
        }

    }
}
