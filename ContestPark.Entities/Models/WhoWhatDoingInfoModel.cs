using ContestPark.Entities.Enums;

namespace ContestPark.Entities.Models
{
    public class PostInfoModel
    {
        public string ContestantId { get; set; }
        public string UserId { get; set; }
        public PostTypes PostTypes { get; set; }
    }
}