using ContestPark.Entities.Models;
using ContestPark.Mobile.Converters;
using ContestPark.Mobile.ViewModels;
using ContestPark.Mobile.Views;
using Xamarin.Forms;

namespace ContestPark.Mobile.Components
{
    public class ChatDetailView : ContentView
    {
        #region Methods

        private void CreateListItem()
        {
            if (Model == null) return;
            var r = ((ChatDetailsPage)ContestParkApp.Current.MainPage).BindingContext as ChatDetailsPageViewModel;
            bool isSenderUser = !(Model.SenderId == r._userDataModule.UserModel.UserId);//Mesajı gönderen karşı taraf ise true

            #region Grid info

            Grid grid = new Grid()
            {
                Padding = new Thickness(10),
                RowSpacing = 0
            };
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            // Column definitions
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = isSenderUser ? 50 : new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = !isSenderUser ? 50 : new GridLength(1, GridUnitType.Star) });

            #endregion Grid info

            #region Color and LayoutOptions settings

            Color textColor = (Color)(isSenderUser ? Application.Current.Resources["DarkGray"] : Application.Current.Resources["White"]);
            Color messgeBackgroundColor = (Color)(isSenderUser ? Application.Current.Resources["Gray"] : Application.Current.Resources["Blue"]);
            LayoutOptions lyoutOptions = isSenderUser ? LayoutOptions.StartAndExpand : LayoutOptions.EndAndExpand;

            #endregion Color and LayoutOptions settings

            #region Circle image

            CircleImage profilePicture = new CircleImage()
            {
                Source = Model.PicturePath,
                VerticalOptions = LayoutOptions.Start,
            };
            //////////////profilePicture.GestureRecognizers.Add(new TapGestureRecognizer()
            //////////////{
            //////////////    TappedCallback = (t, e) => MessagingCenter.Send(new MessagingCenterPush(new ProfilePage() { BindingContext = new ProfilePageViewModel(Model.UserName) }), "MessagingCenterPush")
            //////////////});

            #endregion Circle image

            #region Label message

            HtmlLabel lblMessage = new HtmlLabel()
            {
                FontSize = 16,
                Text = Model.Message,
                LineBreakMode = LineBreakMode.WordWrap,
                TextColor = textColor,
            };

            #endregion Label message

            #region Label date

            Label lblDate = new Label()
            {
                FontSize = 13,
                TextColor = textColor,
                Text = DateTimeMomentConverter.GetPrettyDate(Model.Date),
            };

            #endregion Label date

            #region Root stack

            StackLayout stack = new StackLayout()
            {
                Spacing = 0,

                Padding = new Thickness(5),
                HorizontalOptions = lyoutOptions,
                BackgroundColor = messgeBackgroundColor,
                Children =
                    {
                        lblMessage,
                        lblDate
                    }
            };

            #endregion Root stack

            #region Left or right

            if (isSenderUser)
            {
                grid.Children.Add(profilePicture, 0, 0);
                grid.Children.Add(stack, 1, 0);
            }
            else
            {
                grid.Children.Add(profilePicture, 1, 0);
                grid.Children.Add(stack, 0, 0);
            }

            #endregion Left or right

            Grid.SetRowSpan(profilePicture, 2);//Profil resim için row span ekledim
            Content = grid;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// ChatHistoryModel propery binding
        /// </summary>
        public static readonly BindableProperty ModelProperty = BindableProperty.Create(propertyName: nameof(Model),
                                                                                                returnType: typeof(ChatHistoryModel),
                                                                                                declaringType: typeof(ChatDetailView),
                                                                                                defaultValue: null);

        /// <summary>
        /// ChatHistoryModel binding
        /// </summary>
        public ChatHistoryModel Model
        {
            get { return (ChatHistoryModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        #endregion Properties

        #region Override

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            CreateListItem();
        }

        #endregion Override
    }
}