using System;

namespace ContestPark.Entities.Models
{
    public class UserBlockListModel : BaseModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public DateTime BlockDate { get; set; }
    }
}