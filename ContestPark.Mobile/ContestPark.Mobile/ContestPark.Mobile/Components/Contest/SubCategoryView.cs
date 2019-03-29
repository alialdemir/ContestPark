using ContestPark.Entities.Models;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ContestPark.Mobile.Components
{
    public class SubCategoryView : ContentView
    {
        private bool IsBusy = false;

        #region Methods

        private void CategoryCreator()
        {
            CategoryModel categoryModel = BindingContext as CategoryModel;
            if (categoryModel == null || categoryModel.SubCategories == null) return;

            StackLayout stackmain = new StackLayout() { Spacing = 0, Orientation = StackOrientation.Vertical, BackgroundColor = Color.FromHex("#fff") };
            ScrollView scrollSubCategory = new ScrollView() { Orientation = ScrollOrientation.Horizontal };
            StackLayout stackSubCategory = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0 };
            bool isLock = categoryModel.SubCategories.Any(p => p.DisplayPrice != "0");
            foreach (var subCategory in categoryModel.SubCategories)
            {
                #region Grid info

                CustomGrid subCategoryGrid = new CustomGrid() { Padding = new Thickness(10, 0, 0, 0), ColumnSpacing = 0.1 };
                // Rows
                subCategoryGrid.RowDefinitions.Add(new RowDefinition() { Height = isLock ? 40 : 0 });
                subCategoryGrid.RowDefinitions.Add(new RowDefinition() { Height = 80 });
                subCategoryGrid.RowDefinitions.Add(new RowDefinition() { Height = 20 });
                // Columns
                subCategoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
                subCategoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });

                #region Sub category click

                subCategoryGrid.SingleTap = new Command(() =>
                {
                    if (IsBusy) return;
                    IsBusy = true;
                    SingleTap.Execute(subCategory);
                    IsBusy = false;
                });

                #endregion Sub category click

                #region Sub category long press

                subCategoryGrid.LongPressed = new Command(() =>
                {
                    if (IsBusy) return;
                    IsBusy = true;
                    LongPressed.Execute(subCategory);
                    IsBusy = false;
                });

                #endregion Sub category long press

                #endregion Grid info

                #region Kategori kilitli ise çıkan kategori fiyatı ve coins resmi

                if (subCategory.DisplayPrice != "0")
                {
                    subCategoryGrid.Children.Add(new Label()
                    {
                        Text = subCategory.DisplayPrice,
                        TextColor = Color.FromHex("#333"),
                        FontAttributes = FontAttributes.Bold,
                        LineBreakMode = LineBreakMode.TailTruncation,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    }, 0, 0);
                    subCategoryGrid.Children.Add(new Image()
                    {
                        Source = "coins.png",
                        HeightRequest = 40,
                        WidthRequest = 40,
                        AutomationId = "imgUnlock"
                    }, 1, 0);
                }

                #endregion Kategori kilitli ise çıkan kategori fiyatı ve coins resmi

                #region Sub category Image

                var imgSubCategory = new FFImageLoading.Forms.CachedImage()
                {
                    WidthRequest = 70,
                    HeightRequest = 70,
                    Source = subCategory.PicturePath,
                    DownsampleToViewSize = true,
                    Transformations = new List<ITransformation>() { new CircleTransformation() },
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromDays(365)
                };
                subCategoryGrid.Children.Add(imgSubCategory, 0, 1);
                Grid.SetColumnSpan(imgSubCategory, 2);

                #endregion Sub category Image

                #region Sub category name label

                Label lblSubCategoryName = new Label()
                {
                    //FontSize = 15,
                    TextColor = Color.FromHex("#333"),
                    Text = subCategory.SubCategoryName,
                    FontAttributes = FontAttributes.Bold,
                    LineBreakMode = LineBreakMode.CharacterWrap,
                    HorizontalTextAlignment = TextAlignment.Center,
                    AutomationId = "lblSubCategoryName"
                };
                subCategoryGrid.Children.Add(lblSubCategoryName, 0, 2);
                Grid.SetColumnSpan(lblSubCategoryName, 2);

                #endregion Sub category name label

                stackSubCategory.Children.Add(subCategoryGrid);
                stackmain.Children.Add(scrollSubCategory);
            }
            scrollSubCategory.Content = stackSubCategory;

            #region Zigzag

            stackmain.Children.Add(new Image()
            {
                Source = "zigzag.png",
                Aspect = Aspect.Fill,
            });

            #endregion Zigzag

            Content = stackmain;
        }

        #endregion Methods

        #region Property

        public static readonly BindableProperty SingleTapProperty = BindableProperty.Create(propertyName: nameof(SingleTap),
                                                                                                returnType: typeof(ICommand),
                                                                                                declaringType: typeof(ICommand),
                                                                                                defaultValue: null);

        public ICommand SingleTap
        {
            get { return (ICommand)GetValue(SingleTapProperty); }
            set { SetValue(SingleTapProperty, value); }
        }

        public static readonly BindableProperty LongPressedProperty = BindableProperty.Create(propertyName: nameof(LongPressed),
                                                                                                returnType: typeof(ICommand),
                                                                                                declaringType: typeof(ICommand),
                                                                                                defaultValue: null);

        public ICommand LongPressed
        {
            get { return (ICommand)GetValue(LongPressedProperty); }
            set { SetValue(LongPressedProperty, value); }
        }

        #endregion Property

        #region Override

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            CategoryCreator();
        }

        #endregion Override
    }
}