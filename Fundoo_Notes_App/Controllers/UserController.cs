using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Fundoo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBl userBl;
        private readonly ILogger<UserController> logger;
        public UserController(IUserBl userBl, ILogger<UserController> logger)
        {
            this.userBl = userBl;
            this.logger = logger;
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUser(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                var result = userBl.Registration(userRegistrationModel);
                if (result != null)
                {
                    logger.LogInformation("Registeration Sucessfull");
                    return Ok(new { success = true, message = "registration is successfull"});
                }
                else
                {
                    logger.LogError("Registeration Unsuccessfull");
                    return BadRequest(new { success = false, message = "registration is unsuccessfull" });
                }
            }
            catch (System.Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                var result = userBl.UserLogin(userLoginModel);
                if (result != null)
                {
                    logger.LogInformation("Login Sucessfull");
                    return Ok(new { success = true, message = "Login is successfull", data = result });
                }
                else
                {
                    logger.LogError("Login Unsuccessfull");
                    return BadRequest(new { success = false, message = "Invalid_Login" });
                }
            }
            catch (System.Exception)
            {
                logger.LogError(ToString());
                throw;
            }

        }
        [HttpPost]
        [Route("ForgetPassword")]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                var result = userBl.ForgetPassword(email);
                if (result != null)
                {
                    logger.LogInformation("Login Sucessfull");
                    return Ok(new { success = true, message = "Email Sent Successfully", data = result });
                }
                else
                {
                    logger.LogError("Login Unsuccessfull");
                    return BadRequest(new { success = false, message = "Email Not Sent" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [Authorize]
        [HttpPost]
        [Route("ResetLink")]
        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                var Email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var result = userBl.ResetPassword(Email, password, confirmPassword);

                if (result != null)
                {
                    logger.LogInformation("Password Reset Sucessfull");
                    return Ok(new { sucess = true, Message = "Password Reset Successfully" });
                }
                else
                {
                    logger.LogError("Password Reset Unsuccessfull");
                    return BadRequest(new { success = false, Message = "Password Reset Unsuccessful" });
                }
            }
            catch (System.Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }

      //  long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
    }
}
