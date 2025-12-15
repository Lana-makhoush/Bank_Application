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
                    MustChangePassword = false,
                    CreatedAt = DateTime.Now
                };

                admin.Password = BCrypt.Net.BCrypt.HashPassword ("admin123");


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

                manager.Password = BCrypt.Net.BCrypt.HashPassword("manager123");

                context.Employees.Add(manager);
            }

            context.SaveChanges();
        }
    }
}
