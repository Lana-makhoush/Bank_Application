using Bank_Application.Data;
using Bank_Application.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Bank_Application.Seeders
{
    public static class EmployeeSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            var hasher = new PasswordHasher<Employee>();

            if (!context.Employees.Any(e => e.Username == "admin"))
            {
                var admin = new Employee
                {
                    Username = "admin",
                    FirstName = "System",
                    LastName = "Admin",
                    IdentityNumber = "111111111111",
                    Phone = "0912345678",
                    Role = "Admin",
                    CreatedAt = DateTime.Now
                };

                admin.Password = hasher.HashPassword(admin, "admin123");

                context.Employees.Add(admin);
            }

            if (!context.Employees.Any(e => e.Username == "manager"))
            {
                var manager = new Employee
                {
                    Username = "manager",
                    FirstName = "Main",
                    LastName = "Manager",
                    IdentityNumber = "222222222222",
                    Phone = "0998765432",
                    Role = "Manager",
                    CreatedAt = DateTime.Now
                };

                manager.Password = hasher.HashPassword(manager, "manager123");

                context.Employees.Add(manager);
            }

            context.SaveChanges();
        }
    }
}
