using ContestPark.Mobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContestPark.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuelResultPage : ContentPage
    {
        #region Constructor

        public DuelResultPage()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Methods

        private void ExecuteChangeListTypeCommand(string selectedListTypeId)
        {
            byte selectedListTypeId1 = Convert.ToByte(selectedListTypeId);
            if (selectedListTypeId1 == (byte)DuelResultPageViewModel.ListTypes.DuelResult)
            {
                gridDuelResult.IsVisible = true;
                ////////////////////////////////////////////cvcQuestions.IsVisible = false;
                boxOtherUser.IsVisible = true;
                boxFollowing.IsVisible = false;
            }
            else if (selectedListTypeId1 == (byte)DuelResultPageViewModel.ListTypes.Questions)
            {
                gridDuelResult.IsVisible = false;
                ///////////////////////////////////////  cvcQuestions.IsVisible = true;
                boxOtherUser.IsVisible = false;
                boxFollowing.IsVisible = true;
            }
        }

        #endregion Methods

        #region Commands

        private Command<string> changeListTypeCommand;

        public Command<string> ChangeListTypeCommand
        {
            get { return changeListTypeCommand ?? (changeListTypeCommand = new Command<string>((selectedListTypeId) => ExecuteChangeListTypeCommand(selectedListTypeId))); }
        }

        #endregion Commands
    }
}