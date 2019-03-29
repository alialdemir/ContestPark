using System;
using Xamarin.Forms;

namespace ContestPark.Mobile.Components
{
    public class CustomEditor : Editor
    {
        #region Variable

        private bool sized = false;
        public double lineHeight = 0;

        #endregion Variable

        #region Override

        protected override void OnSizeAllocated(double width, double height)
        {
            if (!sized)
            {
                int count = 0;
                if (Text != null)
                {
                    string[] x = Text.Split(new string[] { "\n" }, StringSplitOptions.None);
                    count = x.Length;
                }
                lineHeight = (height / (count + 1));
                sized = true;
            }
            base.OnSizeAllocated(width, height);
        }

        #endregion Override

        #region Property

        public static readonly BindableProperty PlaceholderBindableProperty = BindableProperty.Create(propertyName: nameof(Placeholder),
                                                                                                returnType: typeof(string),
                                                                                                declaringType: typeof(CustomEditor),
                                                                                                defaultValue: String.Empty);

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderBindableProperty); }
            set { SetValue(PlaceholderBindableProperty, value); }
        }

        #endregion Property
    }
}