﻿using API.Contracts;
using API.Data;
using API.Models;
using System.Linq.Expressions;

namespace API.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BookingDbContext context) : base(context)
        {
        }

        public IEnumerable<Employee> GetByFirstName(string name)
        {
            return _context.Set<Employee>().Where(employee => employee.FirstName.Contains(name));
        }


        public Employee? GetByEmail(string email)
        {
            return _context.Set<Employee>().SingleOrDefault(u => u.Email == email);
        }

        public Employee? GetByEmailAndPhoneNumber(string data)
        {
            return _context.Set<Employee>().FirstOrDefault(e => e.PhoneNumber == data || e.Email == data);
        }

        public Employee? CheckEmail(string email)
        {
           return _context.Set<Employee>().FirstOrDefault(e => e.Email == email);
        }
    }
}
