namespace ContestPark.DataAccessLayer.Tables
{
    public partial class NotificationTypeLang : EntityBase
    {
        public int NotificationTypeLangId { get; set; }
        public string NotificationName { get; set; }
        public byte LanguageId { get; set; }
        public Language Language { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}