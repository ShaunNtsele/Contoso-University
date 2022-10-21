namespace ContosoUniversity.Data
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private AppDbContext _appDbContext;
        private IStudentRepository _student;

        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IStudentRepository Student
        {
            get
            {
                if (_student == null)
                {
                    _student = new StudentRepository(_appDbContext);
                }

                return _student;
            }
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
