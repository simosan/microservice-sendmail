using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimUserManager.Models;
using SimUserManager.Services;

namespace SimUserManager.Controllers
{
    public class DepartmentsController(IDepartmentService departmentService) : Controller
    {
        private readonly IDepartmentService _departmentService= departmentService;

        // GET: Departments
        public IActionResult Index()
        {
            // departmentService参照
            var _dept = _departmentService.GetAll();

            return View(_dept);
        }

        // GET: Departments/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Departments departments = _departmentService.GetDepartmentId(id);

            return View(departments);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DepartmentNo,Department,Section")] Departments departments)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _departmentService.Add(departments);
                    _departmentService.Save();
                    return RedirectToAction("Index");
                }
            } catch(DataException)
            {
                ModelState.AddModelError(string.Empty,
                    "保存に失敗しました。再度実行してください, " +
                    "もし、それでも問題が発生するようであれば管理者へ連絡してくだいさい。");
            }

            return View(departments);
        }

        // GET: Departments/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Departments departments = _departmentService.GetDepartmentId(id);
            return View(departments);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("DepartmentNo,Department,Section")] Departments departments)
        {
            if (id != departments.DepartmentNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _departmentService.Update(departments);
                    _departmentService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError(string.Empty,
                        "更新に失敗しました。再度実行してください。" +
                        "もし、それでも問題が発生するようであれば管理者へ連絡してくだいさい。");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(departments);
        }

        // GET: Departments/Delete/5
        public IActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "削除に失敗しました。再度実行してください。" +
                                       "もし、それでも問題が発生するようであれば管理者へ連絡してください。";
            }

            Departments departments = _departmentService.GetDepartmentId(id);

            return View(departments);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                Departments departments = _departmentService.GetDepartmentId(id);
                _departmentService.Delete(id);
                _departmentService.Save();
            }
            catch(DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("index");
        }
    }
}
