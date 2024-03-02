using System.Collections.Generic;
using SimUserManager.Models;

namespace SimUserManager.Services;

public interface IPositionService
{
    //Postionsすべて取得
    IEnumerable<Positions> GetAll();

    Positions GetPositionId(int? id);

    void Add(Positions item);

    void Save();

    void Update(Positions item);

    void Delete(int id);

}
