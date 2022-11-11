using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Account = CloudinaryDotNet.Account;



namespace RepositoryLayer.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRl : IUserRl
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRl"/> class.
        /// </summary>
        /// <param name="fundooContext">The fundoo context.</param>
        /// <param name="configuration">The configuration.</param>
        public UserRl(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        /// <summary>
        /// Registrations the specified user registration model.
        /// </summary>
        /// <param name="userRegistrationModel">The user registration model.</param>
        /// <returns></returns>
        public UserEntity Registration(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = userRegistrationModel.FirstName;
                userEntity.LastName = userRegistrationModel.LastName;
                userEntity.Email = userRegistrationModel.Email;
                userEntity.Password = ConvertToEncrypt(userRegistrationModel.Password);

                fundooContext.UserTable.Add(userEntity);
                int result = fundooContext.SaveChanges();
                if (result != 0)
                {
                    return userEntity;
                }
                else { return null; }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Users the login.
        /// </summary>
        /// <param name="userLoginModel">The user login model.</param>
        /// <returns></returns>
        public string UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                // var user = fundooContext.UserTable.Where(d => d.Email == userLoginModel.Email && d.Password == userLoginModel.Password).Select(d => d.Email).FirstOrDefault();
                var result = fundooContext.UserTable.Where(d => d.Email == userLoginModel.Email && d.Password == ConvertToEncrypt(userLoginModel.Password)).FirstOrDefault();
                if (result != null)
                {
                    var token = GenerateSecurityToken(result.Email, result.UserId);
                    //var result = "Logged In As : " + fundooContext.UserTable.Where(d => d.Email == userLoginModel.Email && d.Password == userLoginModel.Password).Select(d => d.Email).FirstOrDefault();
                    return token;
                }
                else { return null; }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenerateSecurityToken(string email, long userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.configuration[("JWT:Key")]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("UserId",userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
        public string ForgetPassword(string email)
        {
            try
            {
                var emailcheck = fundooContext.UserTable.FirstOrDefault(x => x.Email == email);
                if (emailcheck != null)
                {
                    var token = GenerateSecurityToken(emailcheck.Email, emailcheck.UserId);
                    MSMQModel mSMQModel = new MSMQModel();
                    mSMQModel.sendData2Queue(token);
                    return token.ToString();
                }
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ResetPassword(string email, string password, string confirmPassword)
        {
            try
            {
                if (password.Equals(confirmPassword))
                {
                    var EmailCheck = fundooContext.UserTable.FirstOrDefault(x => x.Email == email);
                    EmailCheck.Password = password;

                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }



        string maskKey = "adef@kfxcbv@";
        public string ConvertToEncrypt(string Password)
        {
            if (string.IsNullOrEmpty(Password)) return "";
            //(Password += null) return
            Password += maskKey;
            var encodedBytepassword = Encoding.UTF8.GetBytes(Password);
            return Convert.ToBase64String(encodedBytepassword);
        }

        public string ConvertToDecrypt(string encodedBytepassword)
        {
            if (string.IsNullOrEmpty(encodedBytepassword)) return "";
            var base64EncodeBytes = Convert.FromBase64String(encodedBytepassword);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            result = result.Substring(0, result.Length - maskKey.Length + 1);
            return result;
        }

    }
}