using System.Collections.Generic;

namespace ContosoUniversity.Models.ViewModels
{
    public class StudentsListViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
