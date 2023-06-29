using API.DTOs.Accounts;
using API.DTOs.Auths;
using API.Services;
using API.Utilities;
using API.Utilities.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpGet("getall-master")]
        public IActionResult GetMaster()
        {
            var entities = _service.GetMaster();

            if (entities == null)
            {
                return NotFound(new ResponseHandler<GetAllMasterDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetAllMasterDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("getmaster-byguid")]
        public IActionResult GetMaster(Guid guid)
        {
            var master = _service.GetMasterByGuid(guid);
            if (master is null)
            {
                return NotFound(new ResponseHandler<GetAllMasterDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<GetAllMasterDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = master
            });
        }


        [Route("register")]
        [HttpPost]
        public IActionResult Register(RegisterDto register)
        {
            var createdRegister = _service.Register(register);
            if (createdRegister == null)
            {
                return BadRequest(new ResponseHandler<RegisterDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Register failed"
                });
            }

            return Ok(new ResponseHandler<RegisterDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully register",
                Data = createdRegister
            });
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDto login)
        {
            LoginDto loginSuccess = new LoginDto();
            try
            {
                loginSuccess = _service.Login(login);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("not found"))
                {
                    return NotFound(new ResponseHandler<LoginDto>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = ex.Message
                    });
                }
                else
                {
                    return BadRequest(new ResponseHandler<LoginDto>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = ex.Message
                    });
                }
            }

            return Ok(new ResponseHandler<LoginDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully login",
                Data = loginSuccess
            });
        }

        [HttpPut("change-password")]
        public IActionResult Update(ChangePasswordDto changePasswordDto)
        {
            var update = _service.ChangePassword(changePasswordDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Email Not Found"
                });
            }

            if (update is 0)
            {
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Otp Doesn't Match"
                });
            }
            if (update is 1)
            {
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Otp has been used"
                });
            }
            if (update is 2)
            {
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Otp already expired"
                });
            }
            return Ok(new ResponseHandler<ChangePasswordDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully Updated"
            });
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(string email)
        {
            var generateOtp = _service.GenerateOtp(email);
            if (generateOtp is null)
            {
                return BadRequest(new ResponseHandler<ForgotPasswordDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Email Not Found"
                });
            }

            return Ok(new ResponseHandler<ForgotPasswordDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Otp is Generated",
                Data = generateOtp
            });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetAccount();

            if (entities == null)
            {
                return NotFound(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<IEnumerable<GetAccountDto>>
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
            var account = _service.GetAccount(guid);
            if (account is null)
            {
                return NotFound(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = account
            });
        }

        [HttpPost]
        public IActionResult Create(CreateAccountDto newAccountDto)
        {
            var createAccount = _service.CreateAccount(newAccountDto);
            if (createAccount is null)
            {
                return BadRequest(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });
            }

            return Ok(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createAccount
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateAccountDto updateAccountDto)
        {
            var update = _service.UpdateAccount(updateAccountDto);
            if (update is -1)
            {
                return NotFound(new ResponseHandler<UpdateAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (update is 0)
            {
                return BadRequest(new ResponseHandler<UpdateAccountDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check your data"
                });
            }
            return Ok(new ResponseHandler<UpdateAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteAccount(guid);

            if (delete is -1)
            {
                return NotFound(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });
            }
            if (delete is 0)
            {
                return BadRequest(new ResponseHandler<GetAccountDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Check connection to database"
                });
            }

            return Ok(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }
    }
}