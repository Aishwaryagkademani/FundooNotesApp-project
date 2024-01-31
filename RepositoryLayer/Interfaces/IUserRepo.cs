using MassTransit;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
       public UserEntity UserRegistration(RegisterModel register);

        public UserEntity GetUserById(int id);
        public IEnumerable<UserEntity> GetAllUsers();

        public string UserLogin(LoginModel loginModel);
        public UserEntity UpdateUser(UpdateUserModel model, int userId);

        public Task<string> ForgotPassword(string emailTo, IBus bus);

        public int DeleteUser(int userid);

        public UserEntity UserRegisterForPresentOrNot(RegisterModel register);

        public bool ResetPassward(string Password, string confirmPassword, int userId);

        public IEnumerable<UserEntity> GetAllUsersStartsWithS(char StartsWith);


    }
}