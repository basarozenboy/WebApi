namespace WebApi.Services;

using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(int id);
    ServiceResult Create(CreateRequest model);
    ServiceResult Update(int id, UpdateRequest model);
    ServiceResult Delete(int id);
    ServiceResult GenerateAutoData(int itemCount);
}

