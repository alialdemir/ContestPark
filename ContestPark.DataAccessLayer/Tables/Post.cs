using System;
using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Post : EntityBase
    {
        public int PostId { get; set; }
        public int PostTypeId { get; set; }
        public PostType PostType { get; set; }
        public string UserId { get; set; }
        public string ContestantId { get; set; }
        public int DuelId { get; set; }//PostTypeId göre duello Id veya picture Id gelir
        public bool ContestantContestStatus { get; set; }//İki tarafda düelloyu yaptımı onu öğrenmek için
        public bool UserDeletingStatus { get; set; }
        public bool ContestantDeletingStatus { get; set; }
        public DateTime Date { get; set; }
        public int SubCategoryId { get; set; }
        public virtual ICollection<Like> Likes { get; set; } = new HashSet<Like>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}