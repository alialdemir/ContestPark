using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using System;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        bool IsUserStatus(string userName);

        bool IsUserNameControl(string userName);

        bool IsEmailControl(string email);

        bool IsUserIdControl(string userId);

        bool IsFacebookRegister(string userId, string facebookId);

        string UserFullName(string userName);

        string UserId(string userName);

        User FacebookLogin(string facebookId);

        User FacebookRegister(FacebookUserViewModel facebookLogin);

        User GetUserByUserName(string userName);

        DateTime UserLastActiveDate(string userName);

        UpdateUserInfoModel GetUpdateUserInfo(string userId);

        void FacebookRegister(string userId, string facebookId);

        string GetProfilePictureByUserId(string userId);

        UserProfilePageModel GetUserProfileInfo(string userName);
    }
}