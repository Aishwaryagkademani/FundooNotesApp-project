using BusinessLayer.Interfaces;
using MassTransit;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepo userRepo;

        public UserBusiness(IUserRepo userRepo)
        {
            this.userRepo= userRepo;
        }

        public UserEntity UserRegistration (RegisterModel register)
        {
            return userRepo.UserRegistration (register);
        }

        public string UserLogin(LoginModel login)
        {
            return userRepo.UserLogin(login);
        }

        public Task<string> ForgotPassword(string emailTo, IBus bus)
        {
            return userRepo.ForgotPassword (emailTo, bus);
        }

        public UserEntity GetUserById(int id)
        {
            return userRepo.GetUserById(id);

        }

        public IEnumerable<UserEntity> GetAllUsers()
        {
            return userRepo.GetAllUsers();
        }

        public UserEntity UpdateUser(UpdateUserModel model, int userId)
        {
            return userRepo.UpdateUser(model, userId);
        }

        public int DeleteUser(int userid)
        {
            return userRepo.DeleteUser(userid);
        }

        public UserEntity UserRegisterForPresentOrNot(RegisterModel register)
        {
            return userRepo.UserRegisterForPresentOrNot(register);

        }

        public IEnumerable<UserEntity> GetAllUsersStartsWithS(char StartsWith)
        {
            return userRepo.GetAllUsersStartsWithS(StartsWith);
        }

        public bool ResetPassward(string Password, string confirmPassword, int userId)
        {
            return userRepo.ResetPassward(Password, confirmPassword, userId);
        }
    }
}
