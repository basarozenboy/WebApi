using AutoMapper;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UserService(
            DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.OrderByDescending(x => x.Id);
        }

        public User GetById(int id)
        {
            return getUser(id);
        }

        public ServiceResult Create(CreateRequest model)
        {
            var result = new ServiceResult();
            try
            {
                // validate
                if (_context.Users.Any(x => x.Email == model.Email))
                    throw new AppException("User with the email '" + model.Email + "' already exists");

                // map model to new user object
                var user = _mapper.Map<User>(model);

                // save user
                _context.Users.Add(user);
                _context.SaveChanges();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public ServiceResult Update(int id, UpdateRequest model)
        {
            var result = new ServiceResult();
            try
            {
                var user = getUser(id);

                // validate
                if (model.Email != user.Email && _context.Users.Any(x => x.Email == model.Email))
                    throw new AppException("User with the email '" + model.Email + "' already exists");

                // copy model to user and save
                _mapper.Map(model, user);
                _context.Users.Update(user);
                _context.SaveChanges();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public ServiceResult Delete(int id)
        {
            var result = new ServiceResult();
            try
            {
                var user = getUser(id);
                _context.Users.Remove(user);
                _context.SaveChanges();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public ServiceResult GenerateAutoData(int itemCount)
        {
            var result = new ServiceResult();
            try
            {
                for (int i = 0; i < itemCount; i++)
                {
                    var user = new User
                    {
                        FirstName = "FirstName" + i,
                        LastName = "LastName" + i,
                        Email = "email" + i + "@example.com"
                    };
                    _context.Users.Add(user);
                }
                _context.SaveChanges();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        // helper methods

        private User getUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
