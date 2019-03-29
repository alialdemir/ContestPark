using ContestPark.Mobile.Droid.CustomRenderer;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CusttomEditorRenderer))]
namespace ContestPark.Mobile.Droid.CustomRenderer
{
    public class CusttomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var element = e.NewElement as CustomEditor;
                this.Control.Hint = element.Placeholder;
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == CustomEditor.PlaceholderBindableProperty.PropertyName)
            {
                var element = this.Element as CustomEditor;
                this.Control.Hint = element.Placeholder;
            }
        }
    }
}