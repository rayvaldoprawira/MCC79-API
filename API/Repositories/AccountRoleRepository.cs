using API.Contracts;
using API.Data;
using API.Models;
using System;

namespace API.Repositories
{
    public class AccountRoleRepository : GeneralRepository<AccountRole>, IAccountRoleRepository
    { 
        public AccountRoleRepository(BookingDbContext context) : base(context)
        {
        }

        public IEnumerable<AccountRole> GetByGuidEmployee(Guid employeeGuid)
        {
            return _context.Set<AccountRole>().Where(ar => ar.AccountGuid == employeeGuid);
        }
    }
}
