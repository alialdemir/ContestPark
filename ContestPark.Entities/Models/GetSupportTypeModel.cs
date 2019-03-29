namespace ContestPark.Entities.Models
{
    public class GetSupportTypeModel : BaseModel
    {
        public string Description { get; set; }
        public byte SupportTypeId { get; set; }
    }
}