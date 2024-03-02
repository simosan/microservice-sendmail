using System.Collections.Generic;
using SimUserManager.Models;

namespace SimUserManager.Services;

public interface IDepartmentService
{
    //Departmentすべて取得
    IEnumerable<Departments> GetAll();

    Departments GetDepartmentId(int? id);

    void Add(Departments item);

    void Save();

    void Update(Departments item);

    void Delete(int id);

}
