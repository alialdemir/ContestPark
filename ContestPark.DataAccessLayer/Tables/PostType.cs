using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class PostType : EntityBase
    {
        public int PostTypeId { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public virtual ICollection<PostTypeLang> PostTypeLangs { get; set; } = new HashSet<PostTypeLang>();
    }
}