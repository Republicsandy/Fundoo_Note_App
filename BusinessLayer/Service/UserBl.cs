using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLayer.Service
{
    public class UserBl : IUserBl
    {
        private readonly IUserRl userRl;
        public UserBl(IUserRl userRl)

        {
            this.userRl = userRl;
        }
        public UserEntity Registration(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                return userRl.Registration(userRegistrationModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                return userRl.UserLogin(userLoginModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string ForgetPassword(string email)
        {
            try
            {
                return userRl.ForgetPassword(email);
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
                return userRl.ResetPassword(email, password, confirmPassword);
            }
            catch (Exception )
            {
                throw;
            }
        }
    }
}
