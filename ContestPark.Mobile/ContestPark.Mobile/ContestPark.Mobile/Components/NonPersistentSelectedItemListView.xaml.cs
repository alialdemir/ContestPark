﻿using ContestPark.Entities.Models;
using Xamarin.Forms;

namespace ContestPark.Mobile.Components
{
    public partial class NonPersistentSelectedItemListView : ListView
    {
        #region Constructors

        public NonPersistentSelectedItemListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            //InitializeComponent();

            // This prevents the ugly default highlighting of the selected cell upon navigating back to a list view.
            // The side effect is that the list view will no longer be maintaining the most recently selected item (if you're into that kind of thing).
            // Probably not the best way to remove that default SelectedItem styling, but simple and straighforward.
            ItemSelected += (sender, e) => SelectedItem = null;
            ItemAppearing += (sender, e) => InfiniteScroll?.Execute(e.Item);
            HasUnevenRows = true;
            IsPullToRefreshEnabled = true;
        }

        #endregion Constructors

        #region Bindable property

        public static readonly BindableProperty IsShowEmptyMessageProperty = BindableProperty.Create(propertyName: nameof(IsShowEmptyMessage),
                                                                                                     returnType: typeof(bool),
                                                                                                     declaringType: typeof(NonPersistentSelectedItemListView),
                                                                                                     defaultValue: false);

        public bool IsShowEmptyMessage
        {
            get { return (bool)GetValue(IsShowEmptyMessageProperty); }
            set { SetValue(IsShowEmptyMessageProperty, value); }
        }

        public static readonly BindableProperty InfiniteScrollProperty = BindableProperty.Create(defaultValue: null,
                                                                                                 returnType: typeof(Command<BaseModel>),
                                                                                                 propertyName: nameof(InfiniteScroll),
                                                                                                 declaringType: typeof(NonPersistentSelectedItemListView));

        /// <summary>
        /// For paging scroll executing command
        /// </summary>
        public Command<BaseModel> InfiniteScroll
        {
            get { return (Command<BaseModel>)GetValue(InfiniteScrollProperty); }
            set { SetValue(InfiniteScrollProperty, value); }
        }

        #endregion Bindable property
    }
}