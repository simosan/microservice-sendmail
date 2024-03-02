using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SimUserManager.Models;
using SimUserManager.Services;

namespace SimUserManager.Controllers
{
    public class UsersController(IUserService userService) : Controller
    {

        private readonly IUserService _userService = userService!;

        // GET: Users
        public IActionResult Index()
        {
            // UserRepository参照
            var _usrs = _userService.GetAll();
            return View(_usrs);
        }

        // GET: Users/Details/nixxxx
        public IActionResult Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserViewModel _usr = _userService.GetDetailUser(id);

            return View(_usr);
        }

        // GET: Users/Create
        // 役職、所属部署、課班はプルダウンリスト化
        public IActionResult Create()
        {
            //Positionsから役職情報を取得しリスト化
            ViewBag.Opts1 = _userService.Positionslist();
            //Departmentsから課班情報を取得しリスト化
            ViewBag.Opts2 = _userService.DepNamelist();

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserViewModel user)
        {
            try{
                _userService.MkUser(user);
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("DBCreateError", "ユーザ登録に失敗しました。" 
                     + e.Message + e.InnerException );
                return View();
            }

            return RedirectToAction("Index");
        }

        // GET: Users/Edit/ni????
        public IActionResult Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserViewModel user = _userService.GetDetailUser(id);
            //Positionsから役職情報を取得しリスト化
            ViewBag.Opts1 = _userService.Positionslist();
            //Departmentsから課班情報を取得しリスト化
            ViewBag.Opts2 = _userService.DepNamelist();

            return View(user);
        }

        // POST: Users/Edit/nc100000
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public IActionResult Edit(string id, [Bind("Userid,Username,Email,Users_position,Users_depname,Users_depgroupname")] Users users)
        public IActionResult Edit(string id, UserViewModel user)
        {
            if (id != user.UserId)
            {
                ModelState.AddModelError("DBUpdateError",
                       "ユーザ属性編集対象のユーザが存在しません");
                return View();
            }
            
            try{
                _userService.UpdateUser(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("DBUpdateError",
                       "保存に失敗しました。再度実行してください, " +
                       "もし、それでも問題が発生するようであれば管理者へ連絡してくだいさい。");
                return View();
            }
            
            return RedirectToAction("Index");
        }

        // GET: Users/Delete/5
        public IActionResult Delete(bool? saveChangesError = false, string? id = null!)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "削除に失敗しました。再度実行してください、 " +
                                       "もし、それでも問題が発生するようであれば管理者へ連絡してくだいさい。";
            }

            var users = _userService.GetUserId(id);

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            try
            {
                _userService.DeleteUser(id);
            }
            catch (DataException e)
            {
                ModelState.AddModelError("DBUpdateError",
                    "削除に失敗しました。再度実行してください, " +
                    "もし、それでも問題が発生するようであれば管理者へ連絡してください。" +
                    e.Message); 
                return View();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("DBUpdateError",
                    "削除に失敗しました。再度実行してください, " +
                    "もし、それでも問題が発生するようであれば管理者へ連絡してください。" +
                    e.Message);
                return View(); 
            }

            return RedirectToAction("index");
        }

        // <summary>
        // 所属部Idより、その関係課班リストをJsonで返す
        // 本メソッドはUsersViewの所属部ViewModel(knockoutjs)から呼び出される
        // SelectList化されたGroupnameをJson形式で返却
        // </summary>
        // <param name="depname">所属部名</param>
        // <returns>(JsonResult)_depgrpSelectList</returns>
        [HttpGet]
        public JsonResult GetDepGroupname(string depname)
        {
            // UserRepositoryからDepartmentsのGroupname（SelectList)を取得する
            var _depgrpSelectList = _userService.DepGrplist(depname);

            return Json(_depgrpSelectList);
        }
    }
}
