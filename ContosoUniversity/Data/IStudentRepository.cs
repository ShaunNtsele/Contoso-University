using ContosoUniversity.Models;
using System.Collections.Generic;

namespace ContosoUniversity.Data
{
    public interface IStudentRepository: IRepositoryBase<Student>
    {
        //IEnumerable<Student> GetAllStudentsAsce(string sortBy, string searchString);
        //IEnumerable<Student> GetAllStudentsDesc(string sortBy, string searchString);
        Student GetStudentWithEnrollmentDetails(int id);
    }
}
