using API.Contracts;
using API.Data;
using API.Models;
using System.Security.Cryptography.X509Certificates;

namespace API.Repositories;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingDbContext context) : base(context) { }

    public IEnumerable<University> GetByName(string name)
    {
        return _context.Set<University>().Where(u => u.Name.Contains(name));
    }
    public University? GetByCodeName(string code, string name)
    {
        return _context.Set<University>().FirstOrDefault(university => university.Code.ToLower()
                                                                    == code.ToLower()
                                                                    && university.Name.ToLower()
                                                                    == name.ToLower());
    }
}


