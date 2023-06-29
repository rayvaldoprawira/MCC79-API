using API.Contracts;
using API.DTOs.Accounts;
using API.Models;
using API.Utilities;
using API.Utilities.Enums;
using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using API.DTOs.Auths;

namespace API.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IEducationRepository _educationRepository;

        public AccountService(IAccountRepository accountRepository,
            IEmployeeRepository employeeRepository,
            IUniversityRepository universityRepository,
            IEducationRepository educationRepository)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _universityRepository = universityRepository;
            _educationRepository = educationRepository;
        }

        public IEnumerable<GetAllMasterDto>? GetMaster()
        {

            var master = (from e in _employeeRepository.GetAll()
                          join education in _educationRepository.GetAll() on e.Guid equals   education.Guid
                          join u in _universityRepository.GetAll() on education.UniversityGuid equals u.Guid
                          select new GetAllMasterDto
                          {
                              Guid = e.Guid,
                              FullName = e.FirstName + e.LastName,
                              Nik = e.Nik,
                              BirthDate = e.BirthDate,
                              Email = e.Email,
                              Gender = e.Gender,
                              HiringDate = e.HiringDate,
                              PhoneNumber = e.PhoneNumber,
                              Major = education.Major,
                              Degree = education.Degree,
                              Gpa = education.Gpa,
                              UniversityName = u.Name
                          }).ToList();

            if (!master.Any())
            {
                return null;
            }
            
            return master;
        }

        public GetAllMasterDto? GetMasterByGuid(Guid guid)
        {
            var master = GetMaster();
            if (master is null)
            {
                return null;
            }
            var masterGetByGuid = master.FirstOrDefault(master => master.Guid == guid);

            return masterGetByGuid;
        }

        public RegisterDto? Register(RegisterDto registerDto)
        {
            EmployeeService employeeService = new EmployeeService(_employeeRepository);
            Employee employee = new Employee
            {
                Guid = new Guid(),
                Nik = employeeService.GenerateNik(),
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

            University university = new University
            {
                Guid = new Guid(),
                Code = registerDto.UniversityCode,
                Name = registerDto.UniversityName
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
                UniversityGuid = university.Guid
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
            };

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return null;
            }

            var createdAccount = _accountRepository.Create(account);
            if (createdAccount is null)
            {
                return null;
            }


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

            return toDto;
        }

        public LoginDto? Login(LoginDto login)
        {
            var emailEmp = _employeeRepository.GetByEmail(login.Email);
            if (emailEmp == null)
            {
                throw new Exception("Email is Not Found !");
            }

            var pass = _accountRepository.GetByGuid(emailEmp.Guid);
            var isPasswordValid = Hashing.ValidatePassword(login.Password, pass.Password);
            if (!isPasswordValid)
            {
                throw new Exception("Password Invalid");
            }

            var toDto = new LoginDto
            {
                Email = login.Email,
                Password = login.Password
            };

            return toDto;
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
        public ForgotPasswordDto GenerateOtp(string email)
        {
            var employee = _employeeRepository.GetAll().SingleOrDefault(account => account.Email == email);
            if (employee is null)
            {
                return null;
            }

            var toDto = new ForgotPasswordDto
            {
                Email = employee.Email,
                Otp = GenerateRandomOTP(),
                ExpiredTime = DateTime.Now.AddMinutes(5)
            };

            var relatedAccount = _accountRepository.GetByGuid(employee.Guid);

            var update = new Account
            {
                Guid = relatedAccount.Guid,
                Password = relatedAccount.Password,
                IsDeleted = relatedAccount.IsDeleted,
                Otp = toDto.Otp,
                IsUsed = relatedAccount.IsUsed,
                ExpiredTime = DateTime.Now.AddMinutes(5)

            };

            var updateResult = _accountRepository.Update(update);

            return toDto;
        }



        private int GenerateRandomOTP()
        {
            Random random = new Random();
            HashSet<int> uniqueDigits = new HashSet<int>();
            while (uniqueDigits.Count < 6)
            {
                int digit = random.Next(0, 9);
                uniqueDigits.Add(digit);
            }

            int generatedOtp = uniqueDigits.Aggregate(0, (acc, digit) => acc * 10 + digit);

            return generatedOtp;
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
