using ContestPark.Entities.Enums;
using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class MissionsPageViewModel : ViewModelBase<MissionModel>
    {
        #region Private varaibles

        private readonly IMissionsService _missionsService;

        #endregion Private varaibles

        #region Constructors

        public MissionsPageViewModel(IMissionsService missionsService)
        {
            Title = ContestParkResources.Missions;
            _missionsService = missionsService;
        }

        #endregion Constructors

        #region Properties

        private string _ListViewHeader;

        public string ListViewHeader
        {
            get { return _ListViewHeader; }
            private set { SetProperty(ref _ListViewHeader, value, nameof(ListViewHeader)); }
        }

        #endregion Properties

        #region Methods

        protected override void Reflesh()
        {
            ListViewHeader = String.Empty;
            base.Reflesh();
        }

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var missionListModel = await _missionsService.MissionListAsync(ServiceModel);
            if (missionListModel.Items != null)
            {
                Items.AddRange(missionListModel.Items);
                ListViewHeader = missionListModel.Count.ToString() + "/" + missionListModel.CompleteMissionCount.ToString();
            }
            ServiceModel = missionListModel;

            await base.LoadItemsAsync();
            IsBusy = false;
        }

        /// <summary>
        /// Yapılmış olan görevin altınını alma
        /// </summary>
        /// <param name="missionId">Görevin Id'si</param>
        /// <returns></returns>
        public async Task ExecuteTakesTaskGoldCommandAsync(int missionId)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            bool isSuccess = await _missionsService.TakesMissionGoldAsync((Missions)missionId);
            if (isSuccess)
            {
                Items
                   .Where(p => p.MissionId == missionId)
                   .FirstOrDefault()
                   .MissionStatus = true;
            }
            IsBusy = false;
        }

        #endregion Methods

        #region Command

        private ICommand _takesTaskGoldCommand;

        public ICommand TakesTaskGoldCommand
        {
            get { return _takesTaskGoldCommand ?? (_takesTaskGoldCommand = new Command<int>(async (missionId) => { await ExecuteTakesTaskGoldCommandAsync(missionId); })); }
        }

        #endregion Command
    }
}