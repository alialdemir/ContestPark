using ContestPark.Entities.Enums;
using SQLite.Net.Attributes;

namespace ContestPark.Mobile.Models
{
    public class LanguageModel
    {
        [PrimaryKey]
        public byte LanguageModelId { get; set; }

        public Languages Language { get; set; }
    }
}