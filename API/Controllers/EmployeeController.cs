using API.DTOs.Employees;
using API.Services;
using API.Utilities;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/employees")]
  /*  [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]*/
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetEmployee();

            if (entities == null)
            {
                return NotFound(new ResponseHandler<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetEmployeeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var employee = _service.GetEmployee(guid);
            if (employee is null)
            {
                return NotFound(new ResponseHandler<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = employee
            });
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto newEmployeeDto)
        {
            var createEmployee = _service.CreateEmployee(newEmployeeDto);
            if (createEmployee is null)
            {
                return BadRequest(new ResponseHandler<GetEmployeeDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createEmployee
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateEmployeeDto updateEmployeeDto)
        {
            var update = _service.UpdateEmployee(updateEmployeeDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandler<UpdateEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return BadRequest(new ResponseHandler<UpdateEmployeeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new ResponseHandler<UpdateEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteEmployee(guid);

            if (delete is -1)
            {
                return NotFound(new ResponseHandler<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return BadRequest(new ResponseHandler<GetEmployeeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }

        [HttpGet("name/{firstName}")]
        public IActionResult GetByFirstName(string firstName)
        {
            var employee = _service.GetEmploye(firstName);
            if (employee == null)
            {
                return NotFound(new ResponseHandler<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data By Name Not Found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetEmployeeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data By Name Found",
                Data = employee
            });
        }

        [HttpGet("get-all-master")]

        public IActionResult GetMaster()
        {
            var master = _service.GetMaster();
            if (master is null)
            {
                return NotFound(new ResponseHandler<GetAllMasterDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetAllMasterDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = master
            });
        }

        [HttpGet("get-master/{guid}")]

        public IActionResult GetMasterByGuid(Guid guid)
        {
            var masterGuid = _service.GetMasterByGuid(guid);
            if (masterGuid is null)
            {
                return NotFound(new ResponseHandler<GetAllMasterDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new ResponseHandler<GetAllMasterDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = masterGuid
            });
        }
    }
}