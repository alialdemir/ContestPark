using ContestPark.DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public class User : IdentityUser, IEntity
    {
        public string FaceBookId { get; set; }
        public string FullName { get; set; }
        public string ProfilePicturePath { get; set; }
        public string CoverPicturePath { get; set; }
        public bool Status { get; set; }
        public string LanguageCode { get; set; }
        public DateTime RegistryDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public virtual ICollection<Cp> CpS { get; set; } = new HashSet<Cp>();
        public virtual ICollection<Like> Likes { get; set; } = new HashSet<Like>();
        public virtual ICollection<ChatReply> ChatReplys { get; set; } = new HashSet<ChatReply>();
        public virtual ICollection<AskedQuestion> AskedQuestions { get; set; } = new HashSet<AskedQuestion>();
        public virtual ICollection<Chat> Receivers { get; set; } = new HashSet<Chat>();
        public virtual ICollection<Chat> Senders { get; set; } = new HashSet<Chat>();
        public virtual ICollection<Duel> Founders { get; set; } = new HashSet<Duel>();
        public virtual ICollection<Duel> Competitors { get; set; } = new HashSet<Duel>();
        public virtual ICollection<Score> Scores { get; set; } = new HashSet<Score>();
        public virtual ICollection<Setting> Settings { get; set; } = new HashSet<Setting>();
        public virtual ICollection<Support> Supports { get; set; } = new HashSet<Support>();
        public virtual ICollection<Follow> FollowedUsers { get; set; } = new HashSet<Follow>();
        public virtual ICollection<Follow> FollowUpUsers { get; set; } = new HashSet<Follow>();
        public virtual ICollection<FollowCategory> FollowCategories { get; set; } = new HashSet<FollowCategory>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<OpenSubCategory> OpenSubCategories { get; set; } = new HashSet<OpenSubCategory>();
        public virtual ICollection<CompletedMission> CompletedMissions { get; set; } = new HashSet<CompletedMission>();
        public virtual ICollection<ChatBlock> Whons { get; set; } = new HashSet<ChatBlock>();
        public virtual ICollection<ChatBlock> Whos { get; set; } = new HashSet<ChatBlock>();
        public virtual ICollection<Notification> NotificationWhons { get; set; } = new HashSet<Notification>();
        public virtual ICollection<Notification> NotificationWhos { get; set; } = new HashSet<Notification>();

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager,
        //   string authenticationType)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
    }
}