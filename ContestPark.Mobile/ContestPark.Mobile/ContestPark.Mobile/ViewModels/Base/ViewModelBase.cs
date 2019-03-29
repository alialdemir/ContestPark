using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.ViewModels.Base
{
    public abstract class ViewModelBase : BindableBase, INavigationAware
    {
        #region Private varaibles

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        #endregion Private varaibles

        #region Constructors

        public ViewModelBase(INavigationService navigationService = null, IPageDialogService dialogService = null)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            IsBusy = false;
            Title = String.Empty;
            IsShowEmptyMessage = false;
        }

        #endregion Constructors

        #region Page settings

        public bool IsInitialized { get; set; } = false;
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isShowEmptyMessage;

        public bool IsShowEmptyMessage
        {
            get { return _isShowEmptyMessage; }
            set { SetProperty(ref _isShowEmptyMessage, value); }
        }

        #endregion Page settings

        #region Virtoal methods

        /// <summary>
        /// Sayfalarda ortak load işlemleri burada yapılmalı ve refleshs olunca da bu çağrılır
        /// </summary>
        /// <returns></returns>
        protected virtual Task LoadItemsAsync()
        {
            return Task.FromResult(false);
        }

        #endregion Virtoal methods

        #region Dialogs

        protected Task<string> DisplayActionSheetAsync(string title, string cancelButton, string destroyButton, params string[] otherButtons)
        {
            return _dialogService?.DisplayActionSheetAsync(title, cancelButton, destroyButton, otherButtons);
        }

        protected Task DisplayActionSheetAsync(string title, params IActionSheetButton[] buttons)
        {
            return _dialogService?.DisplayActionSheetAsync(title, buttons);
        }

        protected Task<bool> DisplayAlertAsync(string title, string message, string acceptButton, string cancelButton)
        {
            return _dialogService?.DisplayAlertAsync(title, message, acceptButton, cancelButton);
        }

        protected Task DisplayAlertAsync(string title, string message, string cancelButton)
        {
            return _dialogService?.DisplayAlertAsync(title, message, cancelButton);
        }

        #endregion Dialogs

        #region Navigations

        public Task PushModalAsync(string name, NavigationParameters parameters = null)
        {
            if (String.IsNullOrEmpty(name))
                return Task.FromResult(false);

            return _navigationService?.NavigateAsync(name, parameters, useModalNavigation: true);
        }

        public Task PushNavigationPageAsync(string name, NavigationParameters parameters = null, bool? useModalNavigation = false)
        {
            if (String.IsNullOrEmpty(name))
                return Task.FromResult(false);

            return _navigationService?.NavigateAsync(name, parameters, useModalNavigation);
        }

        public Task PushPopupPageAsync(string name, NavigationParameters parameters = null)
        {
            if (String.IsNullOrEmpty(name))
                return Task.FromResult(false);

            return _navigationService?.PushPopupPageAsync(name, parameters);
        }

        public Task PopupGoBackAsync()
        {
            return _navigationService?.PopupGoBackAsync();
        }

        public Task GoBackAsync()
        {
            return _navigationService?.GoBackAsync();
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            if (IsInitialized)
                return;

            LoadItemsCommand.Execute(null);
            IsInitialized = true;
        }

        #endregion Navigations

        #region Commands

        /// <summary>
        /// Veri yükle command
        /// </summary>
        public ICommand LoadItemsCommand => new Command(async () => await LoadItemsAsync());

        #endregion Commands
    }

    public abstract class ViewModelBase<TModel> : ViewModelBase where TModel : BaseModel
    {
        #region Constructors

        public ViewModelBase(INavigationService navigationService = null, IPageDialogService dialogService = null) : base(navigationService, dialogService)
        {
        }

        #endregion Constructors

        #region Virtual methods

        private ObservableRangeCollection<TModel> items;

        /// <summary>
        /// Listview içerisine bind edilecek liste
        /// </summary>
        public ObservableRangeCollection<TModel> Items
        {
            get { return items ?? (items = new ObservableRangeCollection<TModel>()); }
        }

        protected ServiceModel<TModel> _serviceModel;

        public ServiceModel<TModel> ServiceModel
        {
            get
            {
                return _serviceModel ?? (_serviceModel = new ServiceModel<TModel>());
            }
            protected set { if (value != null) _serviceModel = value; }
        }

        /// <summary>
        /// Sayfa reflesh olunca yapılması gereken ortak işlemler
        /// </summary>
        protected virtual void Reflesh()
        {
            IsShowEmptyMessage = false;
            Items?.Clear();
            if (_serviceModel != null)
            {
                _serviceModel.Count = 0;
                _serviceModel.Items = null;
                _serviceModel.PageNumber = 1;
            }
            LoadItemsCommand.Execute(null);
        }

        /// <summary>
        /// Sayfalarda ortak load işlemleri burada yapılmalı ve refleshs olunca da bu çağrılır
        /// </summary>
        /// <returns></returns>
        protected override Task LoadItemsAsync()
        {
            if (ServiceModel != null && ServiceModel.Items != null && ((List<TModel>)ServiceModel.Items).Count > 0) Items.AddRange(ServiceModel.Items);
            else IsShowEmptyMessage = true;

            if (ServiceModel != null && !ServiceModel.IsLastPage)
                ServiceModel.PageNumber++;
            ServiceModel.Items = null;
            return base.LoadItemsAsync();
        }

        #endregion Virtual methods

        #region Paging

        /// <summary>
        /// Sayfalama için scrollbar aşayağıya gelince tekiklenen command
        /// </summary>
        public Command<BaseModel> InfiniteScroll
        {
            get
            {
                return new Command<BaseModel>((currentItem) =>
                {
                    if (ServiceModel.IsLastPage || !(currentItem is BaseModel)) return;
                    if (Items.LastOrDefault().Equals(currentItem)) LoadItemsCommand.Execute(null);
                });
            }
        }

        #endregion Paging

        #region Commands

        /// <summary>
        /// Reflesh command
        /// </summary>
        public ICommand RefleshCommand => new Command(() => Reflesh());

        #endregion Commands
    }
}