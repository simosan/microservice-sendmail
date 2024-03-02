using SimUserManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace SimUserManager.Services;

public class PositionService(UsermanagerContext context) : IPositionService
{
    private readonly UsermanagerContext _context = context;

    public IEnumerable<Positions> GetAll()
    {
        return _context.Positions.ToList();
    }

    public Positions GetPositionId(int? id)
    {
        Positions? positions = _context.Positions.Find(id);
        return positions!;
    }

    public void Add(Positions item)
    {
        _context.Positions.Add(item);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Update(Positions item)
    {
        _context.Entry(item).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
        Positions? positions = _context.Positions.Find(id);
        if ( positions is not null)
        {
            _context.Positions.Remove(positions);
        }
        else
        {
            throw new DataException();
        }  
        _context.Positions.Remove(positions);
    }

}
