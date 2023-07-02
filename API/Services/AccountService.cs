using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.AccountRoles;
using API.Models;
using API.Utilities;
using API.Utilities.Enums;
using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using API.DTOs.Auths;
using System.Security.Claims;
using API.Repositories;
using static System.Net.WebRequestMethods;
using API.Data;
using System.Data;
using System.Linq;
using API.DTOs.Employees;

namespace API.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly ITokenHandler _tokenHandler;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly BookingDbContext _bookingDbContext;

        public AccountService(IAccountRepository accountRepository,
            IEmployeeRepository employeeRepository,
            IUniversityRepository universityRepository,
            IEducationRepository educationRepository,
            ITokenHandler tokenHandler,
            IAccountRoleRepository accountRoleRepository,
            IRoleRepository roleRepository,
            IEmailHandler emailHandler,
            BookingDbContext bookingDbContext)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _universityRepository = universityRepository;
            _educationRepository = educationRepository;
            _tokenHandler = tokenHandler;
            _accountRoleRepository = accountRoleRepository;
            _roleRepository = roleRepository;
            _emailHandler = emailHandler;
            _bookingDbContext = bookingDbContext;
        }


        public RegisterDto? Register(RegisterDto registerDto)
        {
            EmployeeService employeService = new EmployeeService(_employeeRepository, _educationRepository, _universityRepository, _accountRepository, _accountRoleRepository, _roleRepository);

            using var transaction = _bookingDbContext.Database.BeginTransaction();
            try
            {
                Employee employee = new Employee
                {
                    Guid = new Guid(),
                    Nik = employeService.GenerateNik(),
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    BirthDate = registerDto.BirthDate,
                    Gender = registerDto.Gender,
                    HiringDate = registerDto.HiringDate,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                var createdEmployee = _employeeRepository.Create(employee);
                if (createdEmployee is null)
                {
                    return null;
                }

                var universityEntity = _universityRepository.GetByCodeName(registerDto.UniversityCode, registerDto.UniversityName);
                University university = new University
                {
                    Guid = new Guid(),
                    Code = registerDto.UniversityCode,
                    Name = registerDto.UniversityName,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                var createdUniversity = _universityRepository.Create(university);
                if (createdUniversity is null)
                {
                    return null;
                }

                Education education = new Education
                {
                    Guid = employee.Guid,
                    Major = registerDto.Major,
                    Degree = registerDto.Degree,
                    Gpa = registerDto.Gpa,
                    UniversityGuid = university.Guid,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now

                };

                var createdEducation = _educationRepository.Create(education);
                if (createdEducation is null)
                {
                    return null;
                }

                Account account = new Account
                {
                    Guid = employee.Guid,
                    Password = Hashing.HashPassword(registerDto.Password),
                    ConfirmPassword = registerDto.ConfirmPassword,
                    IsDeleted = false,
                    IsUsed = false,
                    Otp = 0,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ExpiredTime = DateTime.Now
                };

                var createdAccount = _accountRepository.Create(account);
                if (createdAccount is null)
                {
                    return null;
                }

                var getRoleUser = _roleRepository.GetByName("User");
                _accountRoleRepository.Create(new AccountRole
                {
                    AccountGuid = account.Guid,
                    RoleGuid = getRoleUser.Guid
                });

                var toDto = new RegisterDto
                {
                    FirstName = createdEmployee.FirstName,
                    LastName = createdEmployee.LastName,
                    BirthDate = createdEmployee.BirthDate,
                    Gender = createdEmployee.Gender,
                    HiringDate = createdEmployee.HiringDate,
                    Email = createdEmployee.Email,
                    PhoneNumber = createdEmployee.PhoneNumber,
                    Password = createdAccount.Password,
                    Major = createdEducation.Major,
                    Degree = createdEducation.Degree,
                    Gpa = createdEducation.Gpa,
                    UniversityCode = createdUniversity.Code,
                    UniversityName = createdUniversity.Name
                };

                transaction.Commit();
                return toDto;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return null;
            }
        }

        public string Login(LoginDto login)
        {
            var emailEmp = _employeeRepository.GetByEmail(login.Email);
            if (emailEmp == null)
            {
                return "0";
            }

            var pass = _accountRepository.GetByGuid(emailEmp.Guid);
            if (pass == null)
            {
                return "0";
            }
            var isPasswordValid = Hashing.ValidatePassword(login.Password, pass.Password);
            if (!isPasswordValid)
            {
                return "-1";
            }


            var claims = new List<Claim>()
            {
                new Claim("Nik", emailEmp.Nik),
                new Claim("FullName", $"{emailEmp.FirstName} {emailEmp.LastName}"),
                new Claim("Email", login.Email)
            };

            var getAccountRole = _accountRoleRepository.GetByGuidEmployee(emailEmp.Guid);
            var getRoleNameByAccountRole = from ar in getAccountRole
                                           join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                                           select r.Name;

            claims.AddRange(getRoleNameByAccountRole.Select(role => new Claim(ClaimTypes.Role, role)));
            try
            {
                var getToken = _tokenHandler.GenerateToken(claims);
                return getToken;
            }
            catch
            {
                return "-2";
            }
        }

        public int ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var isExist = _employeeRepository.CheckEmail(changePasswordDto.Email);
            if (isExist is null)
            {
                return -1; //Account Not Found
            }

            var getAccount = _accountRepository.GetByGuid(isExist.Guid);
            if (getAccount.Otp != changePasswordDto.Otp)
            {
                return 0;
            }

            if (getAccount.IsUsed == true)
            {
                return 1;
            }
            if (getAccount.ExpiredTime < DateTime.Now)
            {
                return 2;
            }

            var account = new Account
            {
                Guid = getAccount.Guid,
                IsUsed = getAccount.IsUsed,
                IsDeleted = getAccount.IsDeleted,
                ModifiedDate = getAccount.ModifiedDate,
                CreatedDate = getAccount.CreatedDate,
                Otp = getAccount.Otp,
                ExpiredTime = getAccount.ExpiredTime,
                Password = Hashing.HashPassword(changePasswordDto.NewPassword)
            };

            var isUpdate = _accountRepository.Update(account);
            if (!isUpdate)
            {
                return 0; // Account not updated
            }

            return 3;
        }
        public ForgotPasswordDto ForgotPassword(string email)
        {
            var employee = _employeeRepository.GetAll().SingleOrDefault(account => account.Email == email);
            if (employee is null)
            {
                return null;
            }

            var toDto = new ForgotPasswordDto
            {
                Email = employee.Email,
                Otp = GenerateOTP.GenerateRandomOTP(),
                ExpiredTime = DateTime.Now.AddMinutes(5)
            };

            var relatedAccount = _accountRepository.GetByGuid(employee.Guid);

            var update = new Account
            {
                Guid = relatedAccount.Guid,
                Password = relatedAccount.Password,
                IsDeleted = relatedAccount.IsDeleted,
                Otp = toDto.Otp,
                IsUsed = false,
                ExpiredTime = DateTime.Now.AddMinutes(5)

            };

            var updateResult = _accountRepository.Update(update);

            _emailHandler.SendEmail(toDto.Email,
                           "Forgot Password",
                           $"Your OTP is {toDto.Otp}");

            return toDto;
        }








        public IEnumerable<GetAccountDto>? GetAccount()
        {
            var accounts = _accountRepository.GetAll();
            if (!accounts.Any())
            {
                return null; // No Account  found
            }

            var toDto = accounts.Select(account =>
                                                new GetAccountDto
                                                {
                                                    Guid = account.Guid,
                                                    IsDeleted = account.IsDeleted,
                                                    IsUsed = account.IsUsed,
                                                    Password = account.Password,
                                                    Otp = account.Otp,
                                                    ExpiredTime = DateTime.Now.AddMinutes(5)
                                                }).ToList();

            return toDto; // Account found
        }

        public GetAccountDto? GetAccount(Guid guid)
        {
            var account = _accountRepository.GetByGuid(guid);
            if (account is null)
            {
                return null; // account not found
            }

            var toDto = new GetAccountDto
            {
                Guid = account.Guid,
                IsDeleted = account.IsDeleted,
                IsUsed = account.IsUsed,
            };
            return toDto; // accounts found
        }

        public GetAccountDto? CreateAccount(CreateAccountDto newAccountDto)
        {
            var account = new Account
            {
                Guid = newAccountDto.Guid,
                Otp = newAccountDto.Otp,
                Password = Hashing.HashPassword(newAccountDto.Password),
                IsDeleted = newAccountDto.IsDeleted,
                IsUsed = newAccountDto.IsUsed,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var createdAccount = _accountRepository.Create(account);
            if (createdAccount is null)
            {
                return null; // Account not created
            }

            var toDto = new GetAccountDto
            {
                Guid = createdAccount.Guid,
                IsDeleted = createdAccount.IsDeleted,
                IsUsed = createdAccount.IsUsed,
                Password = createdAccount.Password

            };

            return toDto; // Account created
        }

        public int UpdateAccount(UpdateAccountDto updateAccountDto)
        {
            var isExist = _accountRepository.IsExist(updateAccountDto.Guid);
            if (!isExist)
            {
                return -1; // Account not found
            }

            var getAccount = _accountRepository.GetByGuid(updateAccountDto.Guid);

            var account = new Account
            {
                Guid = updateAccountDto.Guid,
                Otp = updateAccountDto.Otp,
                Password = Hashing.HashPassword(updateAccountDto.Password),
                IsUsed = (bool)updateAccountDto.IsUsed,
                IsDeleted = updateAccountDto.IsDeleted,
                ModifiedDate = DateTime.Now,
                CreatedDate = getAccount!.CreatedDate
            };

            var isUpdate = _accountRepository.Update(account);
            if (!isUpdate)
            {
                return 0; // Account not updated
            }

            return 1;
        }

        public int DeleteAccount(Guid guid)
        {
            var isExist = _accountRepository.IsExist(guid);
            if (!isExist)
            {
                return -1; // Account not found
            }

            var account = _accountRepository.GetByGuid(guid);
            var isDelete = _accountRepository.Delete(account!);
            if (!isDelete)
            {
                return 0; // Account not deleted
            }

            return 1;
        }

    }
}
