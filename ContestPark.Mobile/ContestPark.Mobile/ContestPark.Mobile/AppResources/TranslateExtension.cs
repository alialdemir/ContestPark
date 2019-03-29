using ContestPark.Mobile.AppResources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var c = ContestParkResources.Culture;
            return Text == null ? Text : ContestParkResources.ResourceManager?.GetString(Text, c);
        }
    }
}