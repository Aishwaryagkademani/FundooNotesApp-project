using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;
using RepositoryLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MassTransit;
namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration configuration;
        public UserRepo(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        public UserEntity UserRegistration(RegisterModel register)
        {
            UserEntity entity = new UserEntity();
            entity.FirstName = register.FirstName;
            entity.LastName = register.LastName;
            entity.EmailId = register.EmailId;
             entity.Password =EncodePassword(register.Password);
            entity.Password = EncodePassword(register.Password);
            fundooContext.Users.Add(entity);
            fundooContext.SaveChanges();
            return entity;
        }


        public static string EncodePassword(string password)
        {
            try
            {
                byte[] encode = new byte[password.Length];
                encode = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encode);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GenerateToken(string EmailId,int UserId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",EmailId),
                new Claim("UserId",UserId.ToString())
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public string UserLogin(LoginModel login)
        {
            var checkUser=fundooContext.Users.FirstOrDefault(x=>x.EmailId==login.EmailId && x.Password== EncodePassword(login.Password));
            if(checkUser != null)
            {

                return GenerateToken(checkUser.EmailId,checkUser.UserId);
            }
            else
            {
                return null;
            }
        }

        public UserEntity GetUserById(int id)
        {
            try
            {
                var getRecord = fundooContext.Users.FirstOrDefault(x => x.UserId == id);
                if (getRecord != null)
                {
                    return getRecord;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable<UserEntity> GetAllUsers()
        {
            return fundooContext.Users.ToList();
        }


        public async Task<string> ForgotPassword(string emailTo, IBus bus)
        {
            try
            {
                if (String.IsNullOrEmpty(emailTo))
                {
                    return null;
                }
                Sent send = new Sent();
                send.SendingMail(emailTo);
                Uri uri = new Uri("rabbitmq://localhost/NotesEmail_Query");
                var endPoint = await bus.GetSendEndpoint(uri);
                return "success";

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }




        public UserEntity UpdateUser(UpdateUserModel model, int userId)
        {
            if (userId != null)
            {
                var getRecord = fundooContext.Users.FirstOrDefault(x => x.UserId == userId);
                if (getRecord != null)
                {
                    getRecord.FirstName = model.FirstName;
                    getRecord.LastName = model.LastName;
                    getRecord.EmailId = model.EmailId;
                    fundooContext.Users.Update(getRecord);
                    fundooContext.SaveChanges();
                    return getRecord;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public int DeleteUser(int userid)
        {
            var result=fundooContext.Users.FirstOrDefault(x=>x.UserId== userid);
            if (result != null)
            {
                fundooContext.Users.Remove(result);
                return fundooContext.SaveChanges();
            }
            return 0;
           
        }

        public UserEntity UserRegisterForPresentOrNot(RegisterModel register)
        {
            var result = fundooContext.Users.FirstOrDefault(x => x.EmailId == register.EmailId);
            if (result != null)
            {
                result.FirstName = register.FirstName;
                result.LastName = register.LastName;
                //result.EmailId = register.EmailId;
                result.Password = EncodePassword(register.Password);
                fundooContext.Users.Update(result);
                fundooContext.SaveChanges();
                return result;
            }
            else
            {
                UserEntity entity = new UserEntity();
                entity.FirstName = register.FirstName;
                entity.LastName = register.LastName;
                entity.EmailId = register.EmailId;
                entity.Password = EncodePassword(register.Password);
                fundooContext.Users.Add(entity);
                fundooContext.SaveChanges();
                return entity;
            }

        }

        public IEnumerable<UserEntity> GetAllUsersStartsWithS(char StartsWith)
        {
            IList<UserEntity> SListuser = new List<UserEntity>();
            var listOfUsers = fundooContext.Users.ToList();
            foreach(var i in listOfUsers)
            {
                var Suser = i.FirstName.StartsWith(StartsWith);
                if(Suser == true)
                SListuser.Add(i);
            }
            //var userRes = (from user in fundooContext.Users select user.FirstName).Contains("S%");
            if (SListuser.Count != 0)
                return SListuser;
            else
                return null;
        }


        public bool ResetPassward(string Password, string confirmPassword,int userId) 
        {
            var res = fundooContext.Users.FirstOrDefault(x => x.UserId == userId);
           
                if (Password == confirmPassword)
                {
                    res.Password = EncodePassword(Password);
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                    return false;
           
        }
    }
}

