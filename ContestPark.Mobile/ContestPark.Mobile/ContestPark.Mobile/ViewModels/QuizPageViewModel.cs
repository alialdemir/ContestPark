using ContestPark.Entities.Models;
using ContestPark.Mobile.ViewModels.Base;
using Prism.Navigation;

namespace ContestPark.Mobile.ViewModels
{
    public class QuizPageViewModel : ViewModelBase<RandomQuestionModel>
    {
        #region Private variables

        private int _duelId = 0;

        #endregion Private variables

        #region Constructors

        public QuizPageViewModel(int duelId)
        {
        }

        #endregion Constructors

        #region Navigation

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("DuelId")) _duelId = parameters.GetValue<int>("DuelId");
            base.OnNavigatedTo(parameters);
        }

        #endregion Navigation
    }
}