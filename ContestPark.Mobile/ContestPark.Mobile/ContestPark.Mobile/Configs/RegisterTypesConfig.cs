using Autofac;
using ContestPark.Mobile.Components;
using ContestPark.Mobile.Models;
using ContestPark.Mobile.Modules;
using ContestPark.Mobile.Services;
using ContestPark.Mobile.Services.Base;
using ContestPark.Mobile.Views;
using Prism.Autofac.Forms;

namespace ContestPark.Mobile.Configs
{
    public class RegisterTypesConfig
    {
        public static IContainer Container { get; set; }

        public static void Init(IContainer container)
        {
            Container = container;
            var ContainerBuilder = new ContainerBuilder();

            RegisterTypesConfig config = new RegisterTypesConfig();
            //ContainerBuilder.RegisterPopupNavigatioService();
            config.RegisterTypeForNavigation(Container);
            config.RegisterTypeSingleInstance(ContainerBuilder);
            ContainerBuilder.Update(Container);
        }

        private void RegisterTypeForNavigation(IContainer container)
        {
            container.RegisterTypeForNavigation<AccountSettingsPage>();
            container.RegisterTypeForNavigation<BaseNavigationPage>();
            container.RegisterTypeForNavigation<CategoriesPage>();
            container.RegisterTypeForNavigation<CategorySearchPage>();
            container.RegisterTypeForNavigation<ChatAllPage>();
            container.RegisterTypeForNavigation<ChatDetailsPage>();
            container.RegisterTypeForNavigation<ChatSettingsPage>();
            container.RegisterTypeForNavigation<ContestStatisticsPage>();
            container.RegisterTypeForNavigation<ContestStorePage>();
            container.RegisterTypeForNavigation<DuelEnterScreenPage>();
            container.RegisterTypeForNavigation<DuelResultPage>();
            container.RegisterTypeForNavigation<EnterPage>();
            container.RegisterTypeForNavigation<FollowsPage>();
            container.RegisterTypeForNavigation<ForgetYourPasswordPage>();
            container.RegisterTypeForNavigation<LanguagesPage>();
            container.RegisterTypeForNavigation<MainPage, MainPageViewModel>();
            container.RegisterTypeForNavigation<Views.MasterDetailPage>();// test et
            container.RegisterTypeForNavigation<MasterPage>();
            container.RegisterTypeForNavigation<MissionsPage>();
            container.RegisterTypeForNavigation<NotificationsPage>();
            container.RegisterTypeForNavigation<PostLikesPage>();
            container.RegisterTypeForNavigation<PostsPage>();
            container.RegisterTypeForNavigation<ProfilePage>();
            container.RegisterTypeForNavigation<QuizPage>();
            container.RegisterTypeForNavigation<RankingPage>();
            container.RegisterTypeForNavigation<SettingsPage>();
            container.RegisterTypeForNavigation<SignInPage>();
            container.RegisterTypeForNavigation<SignUpPage>();
            container.RegisterTypeForNavigation<SupportPage>();
            container.RegisterTypeForNavigation<TabPage>();
            container.RegisterTypeForNavigation<UserChatListPage>();
            container.RegisterTypeForNavigation<DuelBettingPopupPage>();
            container.RegisterTypeForNavigation<PhotoModalView>();
        }

        private void RegisterTypeSingleInstance(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<RequestProvider>().As<IRequestProvider>().SingleInstance();
            containerBuilder.RegisterType<ChatsSignalRService>().As<IChatsSignalRService>().SingleInstance();

            containerBuilder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            containerBuilder.RegisterType<BoostsService>().As<IBoostsService>().SingleInstance();
            containerBuilder.RegisterType<ChatBlocksService>().As<IChatBlocksService>().SingleInstance();
            containerBuilder.RegisterType<ChatRepliesService>().As<IChatRepliesService>().SingleInstance();
            containerBuilder.RegisterType<ChatsService>().As<IChatsService>().SingleInstance();
            containerBuilder.RegisterType<CommentsService>().As<ICommentsService>().SingleInstance();
            containerBuilder.RegisterType<CategoryServices>().As<ICategoryServices>().SingleInstance();
            containerBuilder.RegisterType<ContestDatesService>().As<IContestDatesService>().SingleInstance();
            containerBuilder.RegisterType<CoverPicturesService>().As<ICoverPicturesService>().SingleInstance();
            containerBuilder.RegisterType<CpService>().As<ICpService>().SingleInstance();
            containerBuilder.RegisterType<DuelInfosService>().As<IDuelInfosService>().SingleInstance();
            containerBuilder.RegisterType<DuelSignalRService>().As<IDuelSignalRService>().SingleInstance();
            containerBuilder.RegisterType<FollowCategoryService>().As<IFollowCategoryService>().SingleInstance();
            containerBuilder.RegisterType<FollowsService>().As<IFollowsService>().SingleInstance();
            containerBuilder.RegisterType<LanguageService>().As<ILanguageService>().SingleInstance();
            containerBuilder.RegisterType<LikesService>().As<ILikesService>().SingleInstance();
            containerBuilder.RegisterType<MissionsService>().As<IMissionsService>().SingleInstance();
            containerBuilder.RegisterType<NotificationsService>().As<INotificationsService>().SingleInstance();
            containerBuilder.RegisterType<OpenSubCategoryService>().As<IOpenSubCategoryService>().SingleInstance();
            containerBuilder.RegisterType<PicturesService>().As<IPicturesService>().SingleInstance();
            containerBuilder.RegisterType<QuestionsService>().As<IQuestionsService>().SingleInstance();
            containerBuilder.RegisterType<ScoresService>().As<IScoresService>().SingleInstance();
            containerBuilder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();
            containerBuilder.RegisterType<SignalRServiceBase>().As<ISignalRServiceBase>().SingleInstance();
            containerBuilder.RegisterType<SQLiteService<UserModel>>().As<ISQLiteService<UserModel>>().SingleInstance();
            containerBuilder.RegisterType<SQLiteService<LanguageModel>>().As<ISQLiteService<LanguageModel>>().SingleInstance();
            containerBuilder.RegisterType<SubCategoriesService>().As<ISubCategoriesService>().SingleInstance();
            containerBuilder.RegisterType<SupportService>().As<ISupportService>().SingleInstance();
            containerBuilder.RegisterType<UserDataModule>().As<IUserDataModule>().SingleInstance();
            containerBuilder.RegisterType<PostsService>().As<IPostsService>().SingleInstance();
            containerBuilder.RegisterType<DuelModule>().As<IDuelModule>().SingleInstance();
        }
    }
}