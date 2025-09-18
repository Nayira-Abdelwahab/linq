using System;
using System.Linq;
using CompanyEFCore.Models; 

namespace CompanyEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new CompanyDbContext())
            {
                Console.WriteLine("List of Employees:");

                var employees = context.Employees.ToList();

                foreach (var emp in employees)
                {
                    Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, DepartmentID: {emp.DepartmentId}");
                }

                var newEmployee = new Employee
                {
                    Name = "Nayira Abdelwahab",
                    DepartmentId = 1,
                    Salary = 8000
                };

                context.Employees.Add(newEmployee);
                context.SaveChanges();
                Console.WriteLine("New employee added successfully!");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

