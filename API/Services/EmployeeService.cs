using API.Contracts;
using API.DTOs.Employees;
using API.Models;
using API.DTOs.Accounts;

namespace API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IRoleRepository _roleRepository;

    public EmployeeService(IEmployeeRepository employeeRepository,
                           IEducationRepository educationRepository,
                           IUniversityRepository universityRepository,
                           IAccountRepository accountRepository,
                           IAccountRoleRepository accountRoleRepository,
                           IRoleRepository roleRepository)
    {
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
        _accountRepository = accountRepository;
        _accountRoleRepository = accountRoleRepository;
        _roleRepository = roleRepository;
    }

    public IEnumerable<GetEmployeeDto>? GetEmployee()
    {
        var employees = _employeeRepository.GetAll();
        if (!employees.Any())
        {
            return null; // No employee  found
        }

        var toDto = employees.Select(employee =>
                                            new GetEmployeeDto
                                            {
                                                Guid = employee.Guid,
                                                Nik = employee.Nik,
                                                BirthDate = employee.BirthDate,
                                                Email = employee.Email,
                                                FirstName = employee.FirstName,
                                                LastName = employee.LastName,
                                                Gender = employee.Gender,
                                                HiringDate = employee.HiringDate,
                                                PhoneNumber = employee.PhoneNumber
                                            }).ToList();

        return toDto; // employee found
    }

    public GetEmployeeDto? GetEmployee(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return null; // employee not found
        }

        var toDto = new GetEmployeeDto
        {
            Guid = employee.Guid,
            Nik = employee.Nik,
            BirthDate = employee.BirthDate,
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            PhoneNumber = employee.PhoneNumber
        };
        return toDto; // employees found
    }

    public GetEmployeeDto? CreateEmployee(CreateEmployeeDto newEmployeeDto)
    {
        var employee = new Employee
        {
            Guid = new Guid(),
            PhoneNumber = newEmployeeDto.PhoneNumber,
            FirstName = newEmployeeDto.FirstName,
            LastName = newEmployeeDto.LastName,
            Gender = newEmployeeDto.Gender,
            HiringDate = newEmployeeDto.HiringDate,
            Email = newEmployeeDto.Email,
            BirthDate = newEmployeeDto.BirthDate,
            Nik = GenerateNik(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEmployee = _employeeRepository.Create(employee);
        if (createdEmployee is null)
        {
            return null; // employee not created
        }

        var toDto = new GetEmployeeDto
        {
            Guid = employee.Guid,
            Nik = employee.Nik,
            BirthDate = employee.BirthDate,
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            PhoneNumber = employee.PhoneNumber
        };

        return toDto; // employee created
    }

    public int UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
    {
        var isExist = _employeeRepository.IsExist(updateEmployeeDto.Guid);
        if (!isExist)
        {
            return -1; // employee not found
        }

        var getEmployee = _employeeRepository.GetByGuid(updateEmployeeDto.Guid);

        var employee = new Employee
        {
            Guid = updateEmployeeDto.Guid,
            PhoneNumber = updateEmployeeDto.PhoneNumber,
            FirstName = updateEmployeeDto.FirstName,
            LastName = updateEmployeeDto.LastName,
            Gender = updateEmployeeDto.Gender,
            HiringDate = updateEmployeeDto.HiringDate,
            Email = updateEmployeeDto.Email,
            BirthDate = updateEmployeeDto.BirthDate,
            Nik = updateEmployeeDto.Nik,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEmployee!.CreatedDate
        };

        var isUpdate = _employeeRepository.Update(employee);
        if (!isUpdate)
        {
            return 0; // employee not updated
        }

        return 1;
    }

    public int DeleteEmployee(Guid guid)
    {
        var isExist = _employeeRepository.IsExist(guid);
        if (!isExist)
        {
            return -1; // employee not found
        }

        var employee = _employeeRepository.GetByGuid(guid);
        var isDelete = _employeeRepository.Delete(employee!);
        if (!isDelete)
        {
            return 0; // employee not deleted
        }

        return 1;
    }

    public IEnumerable<GetEmployeeDto>? GetEmploye(string firstName)
    {
        var employees = _employeeRepository.GetByFirstName(firstName);
        if (!employees.Any())
        {
            return null;
        }

        var toDto = employees.Select(employee => new GetEmployeeDto
        {
            Guid = employee.Guid,
            Nik = employee.Nik,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
        }).ToList();

        return toDto;
    }

    public OtpResponseDto? GetByEmail(string email)
    {
        var account = _employeeRepository.GetAll()
            .FirstOrDefault(e => e.Email.Contains(email));

        if (account != null)
        {
            return new OtpResponseDto
            {
                Email = account.Email,
                Guid = account.Guid
            };
        }

        return null;
    }


    public string GenerateNik()
    {
        var employee = _employeeRepository.GetAll();
        if (!employee.Any())
        {
            return "111111";
        }

        var LastData = employee.LastOrDefault();
        var Nik = (int.Parse(LastData.Nik) + 1).ToString();

        return Nik;
    }

    public IEnumerable<GetAllMasterDto>? GetMaster()
    {
        var master = (from e in _employeeRepository.GetAll()
                      join education in _educationRepository.GetAll() on e.Guid equals education.Guid
                      join u in _universityRepository.GetAll() on education.UniversityGuid equals u.Guid
                      join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                      join ar in _accountRoleRepository.GetAll() on a.Guid equals ar.AccountGuid
                      join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid


                      select new GetAllMasterDto
                      {
                          Guid = e.Guid,
                          FullName = e.FirstName + " " + e.LastName,
                          Nik = e.Nik,
                          BirthDate = e.BirthDate,
                          Email = e.Email,
                          HiringDate = e.HiringDate,
                          PhoneNumber = e.PhoneNumber,
                          Major = education.Major,
                          Degree = education.Degree,
                          Gpa = education.Gpa,
                          UniversityName = u.Name,
                          Role = r.Name,
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

        var masterByGuid = master.FirstOrDefault(master => master.Guid == guid);

        return masterByGuid;
    }
}
