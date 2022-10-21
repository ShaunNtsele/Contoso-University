using ContosoUniversity.Data;
using ContosoUniversity.Data.DataAccess;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ContosoUniversity.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IRepositoryWrapper _repo;

        public StudentsController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        [TempData] /*Assigned value will only be available until next request */
        public string Message { get; set; }
        public int iPageSize = 5;

        public IActionResult Index(string sortBy = "LastName", string searchString = "", int page = 1)
        {
            IEnumerable<Student> students;
            Expression<Func<Student, Object>> orderBy;
            string orderByDirection;
            int iTotalStudents;

            ViewData["NameSortParam"] = sortBy == "LastName" ? "LastName_desc" : "LastName";
            ViewData["DateSortParam"] = sortBy == "EnrollmentDate" ? "EnrollmentDate_desc" : "EnrollmentDate";
            ViewData["CurrentFilter"] = searchString;
            ViewData["Message"] = Message;

            if (string.IsNullOrEmpty(sortBy))
            {
                sortBy = "LastName";
            }

            if (sortBy.EndsWith("_desc"))
            {
                sortBy = sortBy.Substring(0, sortBy.Length - 5);
                orderByDirection = "desc";
            }
            else
            {
                orderByDirection = "asc";
            }

            orderBy = p => EF.Property<object>(p, sortBy);  //e.g. p => p.LastName

            if (searchString == "")
            {
                iTotalStudents = _repo.Student.FindAll().Count();
                students = _repo.Student.GetWithOptions(new QueryOptions<Student>
                {
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    //Where = s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString),
                    PageNumber = page,
                    PageSize = iPageSize

                });
            }
            else
            {
                iTotalStudents = _repo.Student.FindByCondition(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString)).Count();
                students = _repo.Student.GetWithOptions(new QueryOptions<Student>
                {
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    //The where clause is used to sort the data according to the search string.
                    Where = s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString),
                    PageNumber = page,
                    PageSize = iPageSize
                });
            }


            return View(new StudentsListViewModel
            {
                Students = students,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = iPageSize,
                    TotalItems = iTotalStudents
                }
            });
        }


        public IActionResult Details(int id)
        {
            var student = _repo.Student.GetStudentWithEnrollmentDetails(id);
            if (student == null)
                return NotFound();
            else
            {
                return View(student);
            }
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            ViewBag.Action = "Add";
            return View("Edit", new Student());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            return View(_repo.Student.GetById(id));
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (student.StudentID == 0)
                    {
                        student.EnrollmentDate = DateTime.Now;
                        _repo.Student.Create(student);
                        Message = $"Student {student.LastName} added succesfully.";
                    }
                    else
                    {
                        _repo.Student.Update(student);
                        Message = $"Student {student.LastName} updated succesfully.";
                    }

                    _repo.Save();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            ViewBag.Action = (student.StudentID == 0) ? "Add" : "Edit";
            return View(student);
        }

        public IActionResult Delete(int id)
        {
            var student = _repo.Student.GetById(id);
            if (student == null)
                return NotFound();
            else
            {
                return View(student);
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteStudent(Student student)
        {
            if (student != null)
            {
                _repo.Student.Delete(student);
                _repo.Save();
                Message = $"Student {student.LastName} deleted succesfully.";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to Delete student";
                return View(student);
            }
        }
    }
}
