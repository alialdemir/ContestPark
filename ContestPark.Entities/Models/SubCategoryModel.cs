using System;
using System.ComponentModel;

namespace ContestPark.Entities.Models
{
    public class SubCategoryModel : INotifyPropertyChanged
    {
        public string SubCategoryName { get; set; }
        public int SubCategoryId { get; set; }
        private string picturePath = DefaultImages.DefaultLock;

        public string PicturePath
        {
            get { return picturePath; }
            set
            {
                picturePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PicturePath)));
            }
        }

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

        private string displayPrice = "0";

        // Json ingore yapılabilir
        public string DisplayPrice
        {
            get { return displayPrice; }
            set
            {
                if (value == "0" || value.IndexOf('K') >= 0) displayPrice = value;
                else displayPrice = new GlobalSettingsManager().NumberFormating(Int32.Parse(value));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayPrice)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}