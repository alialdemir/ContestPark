using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class Language : EntityBase
    {
        public byte LanguageId { get; set; }
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public virtual ICollection<QuestionLang> QuestionLangs { get; set; } = new HashSet<QuestionLang>();
        public virtual ICollection<NotificationTypeLang> NotificationTypeLangs { get; set; } = new HashSet<NotificationTypeLang>();
        public virtual ICollection<SubCategoryLang> SubCategoryLangs { get; set; } = new HashSet<SubCategoryLang>();
        public virtual ICollection<CategoryLang> CategoryLangs { get; set; } = new HashSet<CategoryLang>();
        public virtual ICollection<SupportTypeLang> SupportTypeLangs { get; set; } = new HashSet<SupportTypeLang>();
        public virtual ICollection<PostTypeLang> PostTypeLangs { get; set; } = new HashSet<PostTypeLang>();
        public virtual ICollection<MissionLang> MissionLangs { get; set; } = new HashSet<MissionLang>();
    }
}