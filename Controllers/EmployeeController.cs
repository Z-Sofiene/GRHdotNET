using dotNETProject.Models;
using dotNETProject.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotNETProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IRepository<Employee> employeeRepository;

        public EmployeeController(IRepository<Employee> empRepository)
        {
            employeeRepository = empRepository;
        }

        //
        public ActionResult Search(string term)
        {
            var result = employeeRepository.Search(term);
            return View("Index", result);
        }


        //
        public ActionResult Index()
        {
            var employees = employeeRepository.GetAll();
            ViewData["EmployeesCount"] = employees.Count();
            ViewData["SalaryAverage"] = employeeRepository.SalaryAverage();
            ViewData["MaxSalary"] = employeeRepository.MaxSalary();
            ViewData["HREmployeesCount"] = employeeRepository.HrEmployeesCount();
            return View(employees);
        }


        public ActionResult Details(int id)
        {
            var employee = employeeRepository.FindByID(id);
            return View(employee);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            try
            {
                if (employeeRepository.GetAll().Contains(employee))
                {
                    ModelState.AddModelError("Id", "ID déjas existant");
                    return View();
                }
                else if (ModelState.IsValid)
                {
                    employeeRepository.Add(employee);
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public ActionResult Edit(int id)
        {
            var employee = employeeRepository.FindByID(id);


            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employee employee)
        {
            try
            {
                if (employeeRepository.GetAll().Contains(employee))
                {
                    ModelState.AddModelError("Id", "ID déjas existant");
                    return View();
                }
                else if (ModelState.IsValid)
                {
                    employeeRepository.Update(id, employee);
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public ActionResult Delete(int id)
        {
            var employee = employeeRepository.FindByID(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            employeeRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
