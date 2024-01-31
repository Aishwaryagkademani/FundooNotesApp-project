using MassTransit;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IUserBusiness
    {
        public UserEntity UserRegistration(RegisterModel register);

        public string UserLogin(LoginModel login);
        public Task<string> ForgotPassword(string emailTo, IBus bus);

        public UserEntity GetUserById(int id);
        public IEnumerable<UserEntity> GetAllUsers();

        public UserEntity UpdateUser(UpdateUserModel model, int userId);

        public int DeleteUser(int userid);
        public UserEntity UserRegisterForPresentOrNot(RegisterModel register);

        public IEnumerable<UserEntity> GetAllUsersStartsWithS(char StartsWith);

        public bool ResetPassward(string Password, string confirmPassword, int userId);
    }
}
