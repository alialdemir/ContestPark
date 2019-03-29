using System;

namespace ContestPark.Entities.Models
{
    public class UserOnlineStatusModel//Kullanıcı online mı ve ne zaman giriş yapdı en son //profilde kullandık
    {
        public DateTime LastActiveDate { get; set; }
        public bool OnlineStatus { get; set; }
    }
}