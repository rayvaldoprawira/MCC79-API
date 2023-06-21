using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

    //Table
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<University> Universities { get; set; }


    // Other Configuration or Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Constraint Unique
        modelBuilder.Entity<Employee>()
            .HasIndex(e => new
            {
                e.Nik,
                e.Email,
                e.PhoneNumber
            }).IsUnique();

        // University - Education (One to Many)
        modelBuilder.Entity<University>()
            .HasMany(university => university.Educations)
            .WithOne(education => education.University)
            .HasForeignKey(education => education.UniversityGuid);

        /*modelBuilder.Entity<Education>()
                .HasOne(e => e.University)
                .WithMany(u => u.Educations)
                .HasForeignKey(e => e.UniversityGuid)
                .OnDelete(DeleteBehavior.Cascade);*/

        // Education - Employee (One to One)
        modelBuilder.Entity<Education>()
            .HasOne(education => education.Employee)
            .WithOne(employee => employee.Education)
            .HasForeignKey<Education>(education => education.Guid);

        // Employee - Account (One to One)
        modelBuilder.Entity<Employee>()
            .HasOne(employee => employee.Account)
            .WithOne(account => account.Employee)
            .HasForeignKey<Account>(account => account.Guid);

        //Employee - Booking (One to Many)
        modelBuilder.Entity<Employee>()
            .HasMany(employee => employee.Bookings)
            .WithOne(booking => booking.Employee)
            .HasForeignKey(booking => booking.EmployeeGuid);

        //Account - Account Roles (One to Many)
        modelBuilder.Entity<Account>()
            .HasMany(account => account.AccountRoles)
            .WithOne(accountroles => accountroles.Account)
            .HasForeignKey(accountroles => accountroles.AccountGuid);

        //Account Roles - Roles (One to Many)
        modelBuilder.Entity<AccountRole>()
            .HasOne(accountroles => accountroles.Role)
            .WithMany(roles => roles.AccountRoles)
            .HasForeignKey(accountrole => accountrole.RoleGuid);

        //Booking - Room (One to Many)
        modelBuilder.Entity<Booking>()
            .HasOne(booking => booking.Room)
            .WithMany(room => room.Bookings)
            .HasForeignKey (booking => booking.RoomGuid);
    }

}

