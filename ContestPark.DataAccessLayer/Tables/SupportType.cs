using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class SupportType : EntityBase
    {
        public byte SupportTypeId { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Support> Supports { get; set; } = new HashSet<Support>();
        public virtual ICollection<SupportTypeLang> SupportTypeLangs { get; set; } = new HashSet<SupportTypeLang>();
    }
}