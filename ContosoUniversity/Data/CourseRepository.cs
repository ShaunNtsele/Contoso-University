using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public class CourseRepository: RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext appDbContext)
            : base(appDbContext)
        {
        }
    }
}
