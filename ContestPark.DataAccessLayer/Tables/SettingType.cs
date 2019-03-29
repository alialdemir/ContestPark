using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Tables
{
    public partial class SettingType : EntityBase
    {
        public int SettingTypeId { get; set; }
        public string SettingName { get; set; }
        public virtual ICollection<Setting> Settings { get; set; } = new HashSet<Setting>();
    }
}