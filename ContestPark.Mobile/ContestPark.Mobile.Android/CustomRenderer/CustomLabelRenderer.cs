using ContestPark.Mobile.Droid.CustomRenderer;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(CustomLabelRenderer))]
namespace ContestPark.Mobile.Droid.CustomRenderer
{
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null && Element.Text != null)
                Control.SetText(Html.FromHtml(Element.Text), TextView.BufferType.Spannable);
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            //if (e.PropertyName == Label.TextProperty.PropertyName)
            //    Control.SetText(Html.FromHtml(Element.Text), TextView.BufferType.Spannable);
        }
    }
}