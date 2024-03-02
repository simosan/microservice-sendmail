using System.Collections.Generic;
using SimUserManager.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SimUserManager.Services;

public interface IUserService
{
    //Users,Postions,Departmetnsすべて取得
    IEnumerable<UsersViewModel> GetAll();

    Users GetUserId(string? uid);
    UserViewModel GetDetailUser(string? uid);
    public void MkUser(UserViewModel vmusr);
    public void UpdateUser(UserViewModel vmusr);
    public void DeleteUser(string id);
    void Add(Users item);
    void Update(Users item);
    void Save();
    void Delete(string uid);
    //PostionsモデルのPositionnameをドロップダウンリスト化
    SelectList Positionslist();
    //DepartmentsモデルのDepnameをドロップダウンリスト化
    SelectList DepNamelist();
    //DepartmentsモデルのGroupnameをドロップダウンリスト化
    //引数にDepnameに紐づくDepidを指定する
    List<SelectListItem> DepGrplist(string depname);
    public Users ConvertUvmtoUsers(UserViewModel uvm);
}
