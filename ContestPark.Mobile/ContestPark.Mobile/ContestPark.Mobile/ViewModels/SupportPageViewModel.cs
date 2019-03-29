using ContestPark.Entities.Models;
using ContestPark.Mobile.AppResources;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.ViewModels.Base;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels
{
    public class SupportPageViewModel : ViewModelBase
    {
        #region Private variable

        private readonly ISupportService _supportService;

        #endregion Private variable

        #region Constructors

        public SupportPageViewModel(IPageDialogService pageDialogService,
                                    ISupportService supportService) : base(dialogService: pageDialogService)
        {
            Title = ContestParkResources.RequestsAndComplaints;
            _supportService = supportService;
        }

        #endregion Constructors

        #region Properties

        private List<GetSupportTypeModel> SupportModels { get; set; }
        private SupportModel _supportModel = new SupportModel();

        public SupportModel SupportModel
        {
            get { return _supportModel; }
            set { SetProperty(ref _supportModel, value, nameof(SupportModel)); }
        }

        private IEnumerable<string> _items;

        public IEnumerable<string> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        #endregion Properties

        #region Methods

        protected override async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            SupportModels = (await _supportService.GetSupportTypesAsync()).ToList();
            Items = SupportModels.Select(p => p.Description).ToList();
            await base.LoadItemsAsync();
            IsBusy = false;
        }

        private async Task ExecuteSendSupportCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (SupportModel.SupportTypeId == -1 || String.IsNullOrEmpty(SupportModel.Message))
            {
                await DisplayAlertAsync(ContestParkResources.Error,
                                        ContestParkResources.SelectATtopicAndWriteYourMessage,
                                        ContestParkResources.Okay);
                IsBusy = false;
                return;
            }

            SupportModel.SupportTypeId = (sbyte)SupportModels[SupportModel.SupportTypeId].SupportTypeId;// Picker den selected index geliyor onu SupportTypeId'ye dönüştürdüm

            var isSuccess = await _supportService.InsertSupportAsync(SupportModel);
            if (isSuccess)
            {
                SupportModel = new SupportModel();
                await DisplayAlertAsync(ContestParkResources.Success,
                                        ContestParkResources.ServerMessage_supportMessage,
                                        ContestParkResources.Okay);
            }
            IsBusy = false;
        }

        #endregion Methods

        #region Commands

        private ICommand sendSupportCommand;

        public ICommand SendSupportCommand
        {
            get { return sendSupportCommand ?? (sendSupportCommand = new Command(async () => await ExecuteSendSupportCommandAsync())); }
        }

        #endregion Commands
    }
}