using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimUserManager.Models;
using SimUserManager.Services;

namespace SimUserManager.Controllers
{
    public class PositionsController(IPositionService positionService) : Controller
    {
        private readonly IPositionService _positionService = positionService;

        // GET: Positions
        public IActionResult Index()
        {
            // positionService参照
            var _posit = _positionService.GetAll();

            return View(_posit);
        }

        // GET: Positions/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Positions positions = _positionService.GetPositionId(id);

            return View(positions);
        }

        // GET: Positions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PositionNo,PositionName,Rankvalue")] Positions positions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _positionService.Add(positions);
                    _positionService.Save();
                    return RedirectToAction("Index");
                }
            } catch(DataException)
            {
                ModelState.AddModelError(string.Empty,
                    "保存に失敗しました。再度実行してください, " +
                    "もし、それでも問題が発生するようであれば管理者へ連絡してくだいさい。");
            }

            return View(positions);
        }

        // GET: Positions/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Positions positions = _positionService.GetPositionId(id);
            return View(positions);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PositionNo,PositionName,Rankvalue")] Positions positions)
        {
            if (id != positions.PositionNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _positionService.Update(positions);
                    _positionService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError(string.Empty,
                        "更新に失敗しました。再度実行してください。" +
                        "もし、それでも問題が発生するようであれば管理者へ連絡してくだいさい。");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(positions);
        }

        // GET: Positions/Delete/5
        public IActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "削除に失敗しました。再度実行してください。" +
                                       "もし、それでも問題が発生するようであれば管理者へ連絡してください。";
            }

            Positions positions = _positionService.GetPositionId(id);

            return View(positions);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                Positions positions = _positionService.GetPositionId(id);
                _positionService.Delete(id);
                _positionService.Save();
            }
            catch(DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("index");
        }
    }
}
