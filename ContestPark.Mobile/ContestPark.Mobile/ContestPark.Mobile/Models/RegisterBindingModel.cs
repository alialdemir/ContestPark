﻿namespace ContestPark.Mobile.Models
{
    public class RegisterBindingModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string LanguageCode { get; set; }
    }
}