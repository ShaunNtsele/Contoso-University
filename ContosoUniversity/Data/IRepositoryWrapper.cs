namespace ContosoUniversity.Data
{
    public interface IRepositoryWrapper
    {
        IStudentRepository Student { get; }

        void Save();
    }
}
