using ContosoUniversity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ContosoUniversity.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student { Initials = "C", FirstName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2020-02-01") },
                    new Student { Initials = "M", FirstName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2019-02-01") },
                    new Student { Initials = "A", FirstName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2019-02-01") },
                    new Student { Initials = "G", FirstName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2021-02-01") },
                    new Student { Initials = "Y", FirstName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2021-02-01") },
                    new Student { Initials = "P", FirstName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2021-07-01") },
                    new Student { Initials = "L", FirstName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2020-07-01") },
                    new Student { Initials = "N", FirstName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2019-02-01") }

                    );
            }

            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Course { CourseID = 1050, Title = "Chemistry", Credits = 12 },
                    new Course { CourseID = 4022, Title = "Microeconomics", Credits = 12 },
                    new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 12 },
                    new Course { CourseID = 1045, Title = "Calculus", Credits = 16 },
                    new Course { CourseID = 3141, Title = "Trigonometry", Credits = 16 },
                    new Course { CourseID = 2021, Title = "Composition", Credits = 12 },
                    new Course { CourseID = 2042, Title = "Literature", Credits = 16 }
                    );
            }

            if (!context.Enrollments.Any())
            {
                context.Enrollments.AddRange(
                    new Enrollment { StudentID = 1, CourseID = 1050, Grade = Grade.A },
                    new Enrollment { StudentID = 1, CourseID = 4022, Grade = Grade.C },
                    new Enrollment { StudentID = 1, CourseID = 4041, Grade = Grade.B },
                    new Enrollment { StudentID = 2, CourseID = 1045, Grade = Grade.B },
                    new Enrollment { StudentID = 2, CourseID = 3141, Grade = Grade.F },
                    new Enrollment { StudentID = 2, CourseID = 2021, Grade = Grade.F },
                    new Enrollment { StudentID = 3, CourseID = 1050 },
                    new Enrollment { StudentID = 4, CourseID = 1050 },
                    new Enrollment { StudentID = 4, CourseID = 4022, Grade = Grade.F },
                    new Enrollment { StudentID = 5, CourseID = 4041, Grade = Grade.C },
                    new Enrollment { StudentID = 6, CourseID = 1045 },
                    new Enrollment { StudentID = 7, CourseID = 3141, Grade = Grade.A }
                    );
            }

            context.SaveChanges();
        }
    }
}
