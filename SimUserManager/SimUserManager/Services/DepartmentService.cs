using System.Data;
using Microsoft.EntityFrameworkCore;
using SimUserManager.Models;

namespace SimUserManager.Services;

public class DepartmentService(UsermanagerContext context) : IDepartmentService
{
    private readonly UsermanagerContext _context = context;

    public IEnumerable<Departments> GetAll()
    {
        return _context.Departments.ToList();
    }

    public Departments GetDepartmentId(int? id)
    {
        Departments? departments = _context.Departments.Find(id);
        return departments!;
    }

    public void Add(Departments item)
    {
        _context.Departments.Add(item);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Update(Departments item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
        Departments? departments = _context.Departments.Find(id);

        if ( departments is not null)
        {
            _context.Departments.Remove(departments);
        }
        else
        {
            throw new DataException();
        }  

        _context.Departments.Remove(departments);
    }

}
