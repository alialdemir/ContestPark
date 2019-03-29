using System;

namespace ContestPark.Entities.Models
{
    public class SubCategorySearchModel : BaseModel
    {
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        private string picturePath;

        public string PicturePath
        {
            get { return picturePath; }
            set
            {
                if (DisplayPrice != "0")
                {
                    picturePath = DefaultImages.DefaultLock;
                }
                else picturePath = value;
            }
        }

        public int SubCategoryId { get; set; }

        private int price;

        public int Price
        {
            get { return price; }
            set
            {
                price = value;
                DisplayPrice = price.ToString();
            }
        }

        // Json ingore edilebilir.
        private string displayPrice = "0";

        public string DisplayPrice
        {
            get { return displayPrice; }
            set
            {
                if (value == "0" || value.IndexOf('K') >= 0) displayPrice = value;
                else displayPrice = new GlobalSettingsManager().NumberFormating(Convert.ToInt32(value));
            }
        }
    }
}