using API.Models;

namespace API.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        Employee? GetByEmail(string email);
        Employee? GetByEmailAndPhoneNumber(string data);
        Employee? CheckEmail(string email);
        IEnumerable<Employee> GetByFirstName(string name);
    }
}
