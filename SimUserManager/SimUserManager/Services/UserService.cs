using SimUserManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Drawing;

namespace SimUserManager.Services;

public class UserService(UsermanagerContext context, IConfiguration configuration) : IUserService
{
    private readonly UsermanagerContext _context = context;
    private readonly IConfiguration _config = configuration;

    public IEnumerable<UsersViewModel> GetAll()
    {
        // Users、Postions、Departmentsを内部結合
        var model = _context.Users
            .Join(_context.Positions,
                u => u.PositionNo,
                p => p.PositionNo,
                ( u, p ) => new { u, p })
            .Join(_context.Departments,
                up => up.u.DepartmentNo,
                d => d.DepartmentNo,
                ( up, d ) => new { up, d })
            .Select( upd => new UsersViewModel
            {
                UserId = upd.up.u.UserId,
                Lastname = upd.up.u.Lastname,
                Firstname = upd.up.u.Firstname,
                Email = upd.up.u.Email,
                PositionName = upd.up.p.PositionName,
                Department = upd.d.Department,
                Section = upd.d.Section
            });

        return model;
    }

    public Users GetUserId(string? uid)
    {
        Users? usr = _context.Users.Find(uid);
       
        return usr!;
    }

    public UserViewModel GetDetailUser(string? uid)
    {
        // Users、Postions、Departmentsを内部結合し、UserViewModelに格納
        UserViewModel? user = _context.Users
            .Join(_context.Positions,
                u => u.PositionNo,
                p => p.PositionNo,
                ( u, p) => new { u, p })
            .Join(_context.Departments,
                up => up.u.DepartmentNo,
                d => d.DepartmentNo,
                ( up, d ) => new { up, d })
            .Where( upd => upd.up.u.UserId == uid)
            .Select( upd => new UserViewModel
            {
                UserId = upd.up.u.UserId,
                Lastname = upd.up.u.Lastname,
                Firstname = upd.up.u.Firstname,
                Email = upd.up.u.Email,
                PositionName = upd.up.p.PositionName,
                Department = upd.d.Department, 
                Section = upd.d.Section
            })
            .FirstOrDefault();

        return user!;
    }

    private int GetPositionNameToPosiNo(UserViewModel usr) 
    {
        var posino = _context.Positions
            .Where(p => p.PositionName == usr.PositionName)
            .Select(p => new { PositionNo = p.PositionNo })
            .FirstOrDefault();

        if ( posino is not null )
        { 
            return posino.PositionNo;
        }
        else
        {
            return -1;
        }
    }

    private int GetSectionToDepNo(UserViewModel usr) 
    {
      
        var depno = _context.Departments
            .Where(d => d.Department == usr.Department && d.Section == usr.Section)
            .Select(d => new { DepNo = d.DepartmentNo })
            .FirstOrDefault();
        
        if (depno is not null)
        {
            return depno.DepNo;
        }
        else
        {
            return -1;
        }
    }

    //public Users MkUser(UserViewModel vmusr)
    public void MkUser(UserViewModel vmusr)
    {
        Users nwusr = this.ConvertUvmtoUsers(vmusr);
        using (var tran = _context.Database.BeginTransaction())
        {
            try{
                // SimUserManagerのDB更新
                this.Add(nwusr);
                this.Save();
                // Auth0登録
                var auth0client = new Auth0ApiClient(_config!);
                var tk = auth0client.GetAccessToken();
                Auth0UserService uregst = new Auth0UserService(_config!);
                var statuscd = uregst.Auth0UserCreate(vmusr, tk!);
                if ( statuscd != 201){
                    throw new DbUpdateException("DB更新後のAuth0登録に失敗。");
                }

                tran.Commit();
            }
            catch (DbUpdateException)
            {
                tran.Rollback();
                throw; 
            }
        }
    }

    public void UpdateUser(UserViewModel vmusr)
    {
        Users updusr = this.ConvertUvmtoUsers(vmusr);
        try{
            this.Update(updusr);
            this.Save();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }

    public void DeleteUser(string id)
    {
        Users? deluser = this.GetUserId(id);
        using (var tran = _context.Database.BeginTransaction())
        {
            // SimUserManagerのDB更新
            try{
                this.Delete(id);
                this.Save();
                // Auth0削除
                var auth0client = new Auth0ApiClient(_config!);
                var tk = auth0client.GetAccessToken();
                Auth0UserService uregst = new Auth0UserService(_config!);
                var uid = uregst.Auth0UserIdObtain(deluser, tk!);

                var statuscd = uregst.Auth0UserDelete(uid!, tk!);
                Console.WriteLine(statuscd);
                if ( statuscd != 204 ){
                   tran.Rollback();
                   throw new DbUpdateException("DB更新後のAuth0削除に失敗。"); 
                }

                tran.Commit();
            }
            catch (DataException)
            {
                tran.Rollback();
                throw;
            }
        }
    }

    public void Add(Users item)
    {
        _context.Users.Add(item);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Update(Users item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(string uid)
    {
        Users? users = _context.Users.Find(uid);
        if ( users is not null)
        {
            _context.Users.Remove(users);
        }
        else
        {
            throw new DataException();
        }  
    }

    public SelectList Positionslist()
    {
        var posinamelist = _context.Positions
            .Select(p => new { PositionName = p.PositionName })
            .Distinct();
        var opts1 = new SelectList(posinamelist, "PositionName", "PositionName");

        return opts1;

    }

    public SelectList DepNamelist()
    {
        var depnamelist = _context.Departments
           .Select(d => new { Department = d.Department })
           .Distinct();
        var opts2 = new SelectList(depnamelist, "Department", "Department");

        return opts2;
    }

    public List<SelectListItem> DepGrplist(string depname)
    {

        var depgrplist = _context.Departments
            .Where(d => d.Department == depname)
            .Select(d => new SelectListItem()
               { Value = d.Department, Text = d.Section })
            .ToList();

        return depgrplist;

    }

    public Users ConvertUvmtoUsers(UserViewModel uvm)
    {
        int posino = GetPositionNameToPosiNo(uvm);
        int depno = GetSectionToDepNo(uvm);

        Users convuser = new Users();
        convuser.UserId = uvm.UserId;
        convuser.Lastname = uvm.Lastname;
        convuser.Firstname = uvm.Firstname;
        convuser.Email = uvm.Email;
        convuser.PositionNo = posino;
        convuser.DepartmentNo = depno;

        return convuser;
    
    }

}
