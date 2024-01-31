using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        private readonly IBus bus;

        private readonly ILogger<UserController> logger;

        public UserController(IUserBusiness userBusiness, IBus bus, ILogger<UserController> logger)
        {
            this.userBusiness = userBusiness;
            this.bus = bus;

            this.logger = logger;
            //logging using log function
            logger.Log(LogLevel.Information, "Hello from User controller");

            //logging using logInformation method
            logger.LogInformation("Hello again from user controller");
            //logging using logWarning method
            logger.LogWarning("This is the warning message from user controller");
            //logging using logError method
            try
            {
                throw new Exception("This is inside of try block");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "This is in catcch block");
            }

            //logging using logTrace method
            logger.LogTrace("This is in the trace");

            using (logger.BeginScope(new Dictionary<string, object> { { "userId", 5 } }))
            {
                logger.LogInformation("hello");
                logger.LogInformation("world");
            }
        }

        
        [HttpPost]
        [Route("Registration")]
        public IActionResult Register(RegisterModel model)
        {
            var result = userBusiness.UserRegistration(model);
            if (result != null)
            {
                return Ok(new ResponseModel<UserEntity> { Success = true, Message = "registeration is successfull", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Registration not successfull" });
            }
        }

        [HttpGet]
        [Route("Get Users by Id")]
        public IActionResult UserGetById(int id)
        {
            var result = userBusiness.GetUserById(id);
            if (result != null)
            {
                return Ok(new ResponseModel<UserEntity> { Success = true, Message = "registeration is successfull", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Registration not successfull" });
            }
        }

        [HttpGet]
        [Route("Get All Users")]
        public IActionResult GetAllUsersRecords()
        {
            //writing logger for get methods
            logger.LogInformation(1001, "getting the user details {count}",5);

            var result = userBusiness.GetAllUsers();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Registration not successfull" });
            }
        }


        [HttpPut]
        [Route("Update Users")]
        public IActionResult UpdateUserRecords(UpdateUserModel model,int userId)
        {
            var result = userBusiness.UpdateUser(model,userId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Registration not successfull" });
            }
        }




        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel model)
        {
            var result = userBusiness.UserLogin(model);
            if(result != null)
            {
                return Ok(new ResponseModel<string> { Success=true,Message="Login successfull",Data = result});
            }
            else
            {
                return BadRequest("Login is not successfull");
            }
        }

        [HttpPost]
        [Route("forgot password")]
        public IActionResult ForgotPassword(string emailTo)
        {
            try
            {
                var result = userBusiness.ForgotPassword(emailTo, bus);
                if (result != null)
                {
                    return Ok(new { Success = true, Message = "token generated successfully and Authentication Notification send to ", data = result });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Token not Generated" });
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete Users")]
        public IActionResult DeleteUserById(int userId)
        {
            int result = userBusiness.DeleteUser(userId);
            if(result != 0)
            {
                return Ok(new ResponseModel<int>
                {
                    Success = true,Message = "User deleted successfull", Data = result
                });
            }
            else
            {
                return BadRequest(new ResponseModel<int>
                {
                    Success = true,Message = "User deleted successfull",Data = result
                });
            }

        }

        [HttpPost]
        [Route("Registration By checking if user is present or not")]
        public IActionResult RegisterForPresentOrNot(RegisterModel model)
        {
            var result = userBusiness.UserRegisterForPresentOrNot(model);
            if (result != null)
            {
                return Ok(new ResponseModel<UserEntity> { Success = true, Message = "registeration is successfull", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Registration not successfull" });
            }
        }

        [HttpGet]
        [Route("Get Users Whose FirstName starts with S")]
        public IActionResult GetAllRecorsStartsWithS(char StartsWith)
        {
           
            var result = userBusiness.GetAllUsersStartsWithS(StartsWith);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "User not found" });
            }
        }

        [HttpPost]
        [Route("Reset password")]
        public IActionResult ResetPasswardOfUser(string password,string confirmPassword,int userId)
        {
            var result = userBusiness.ResetPassward(password, confirmPassword, userId);
            if (result == true)
            {
                return Ok("Password reset successfull");
            }
            else
                return BadRequest("Password reset not successfull");

        }
    }
}

