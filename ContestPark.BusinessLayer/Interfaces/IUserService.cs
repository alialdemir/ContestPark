using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System;
using System.Threading.Tasks;

namespace ContestPark.BusinessLayer.Interfaces
{
    public interface IUserService : IRepository<User>
    {
        bool IsUserStatus(string userName);

        bool IsUserNameControl(string userName);

        bool IsEmailControl(string email);

        bool IsFacebookRegister(string userId, string facebookId);

        bool IsUserIdControl(string userId);

        string UserFullName(string userName);

        string UserId(string userName);

        User FacebookLogin(string facebookId);

        User FacebookRegister(FacebookUserViewModel facebookLogin);

        User GetUserByUserName(string userName);

        DateTime UserLastActiveDate(string userName);

        UpdateUserInfoModel GetUpdateUserInfo(string userId);

        void FacebookRegister(string userId, string facebookId);

        Task Insert(UserRegisterModel model);

        Task<bool> ForgotYourPasswordAsync(string userNameOrPassword);

        string GetProfilePictureByUserId(string userId);

        UserProfilePageModel GetUserProfileInfo(string userName);
    }
}