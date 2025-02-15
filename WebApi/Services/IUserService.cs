namespace WebApi.Services;

using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(int id);
    ServiceResult Create(CreateUser model);
    ServiceResult Update(int id, UpdateUser model);
    ServiceResult Delete(int id);
    ServiceResult GenerateAutoData(int itemCount);
}

