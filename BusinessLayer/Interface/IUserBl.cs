using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBl
    {
        public UserEntity Registration(UserRegistrationModel userRegistrationModel);
        public string UserLogin(UserLoginModel userLoginModel);
        public string ForgetPassword(string email);
        public bool ResetPassword(string email, string password, string confirmPassword);
    }
}
