namespace ContestPark.WebApi
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ContestParkContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #region AddScoped and AddSingleton and AddTransient

            // Services
            services.AddScoped<IAskedQuestionService, AskedQuestionService>();
            services.AddScoped<IBoostService, BoostService>();
            services.AddScoped<IChatBlockService, ChatBlockService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IChatReplyService, ChatReplyManager>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ICompletedMissionService, CompletedMissionService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IContestDateService, ContestDateService>();
            services.AddScoped<ICpInfoService, CpInfoService>();
            services.AddScoped<ICpService, CpService>();
            services.AddScoped<IDuelInfoService, DuelInfoService>();
            services.AddScoped<IDuelService, DuelService>();
            services.AddScoped<IFollowCategoryService, FollowCategoryService>();
            services.AddScoped<IFollowService, FollowService>();
            services.AddScoped<IGlobalSettings, GlobalSettingsManager>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IMissionService, MissionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IOpenSubCategoryService, OpenSubCategoryService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ISubCategoryLangService, SubCategoryLangService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<ISupportService, SupportService>();
            services.AddScoped<ISupportTypeService, SupportTypeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostTypeService, PostTypeService>();

            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<IFtpService, FtpService>();
            services.AddTransient<IMissionCreator, MissionCreator>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDbFactory, DbFactory>();
            services.AddTransient<IDuelUserService, DuelUserService>();
            services.AddTransient<IGameDuelService, GameDuelService>();

            // Rpoesitory
            services.AddScoped<IAskedQuestionRepository, EfAskedQuestionDal>();
            services.AddScoped<IBoostRepository, EfBoostDal>();
            services.AddScoped<IChatBlockRepository, EfChatBlockDal>();
            //services.AddScoped<IChatRepository, EfChatDal>();
            services.AddScoped<IChatRepository, ChatDapperRepository>();
            services.AddScoped<IChatReplyRepository, EfChatReplyDal>();
            services.AddScoped<ICommentRepository, EfCommentDal>();
            services.AddScoped<ICompletedMissionRepository, EfCompletedMissionDal>();
            services.AddScoped<ICategoryRepository, CategoryDapperRepository>();
            services.AddScoped<IContestDateRepository, EfContestDateDal>();
            services.AddScoped<ICpInfoRepository, EfCpInfoDal>();
            services.AddScoped<ICpRepository, EfCpDal>();
            services.AddScoped<IDuelInfoRepository, EfDuelInfoDal>();
            services.AddScoped<IDuelRepository, EfDuelDal>();
            services.AddScoped<IFollowCategoryRepository, EfFollowCategoryDal>();
            services.AddScoped<IFollowRepository, EfFollowDal>();
            services.AddScoped<ILanguageRepository, EfLanguageDal>();
            services.AddScoped<ILikeRepository, EfLikeDal>();
            services.AddScoped<IMissionRepository, EfMissionDal>();
            services.AddScoped<INotificationRepository, EfNotificationDal>();
            services.AddScoped<IOpenSubCategoryRepository, EfOpenSubCategoryDal>();
            services.AddScoped<IQuestionRepository, EfQuestionDal>();
            services.AddScoped<IScoreRepository, EfScoreDal>();
            services.AddScoped<ISettingRepository, EfSettingDal>();
            services.AddScoped<ISubCategoryLangRepository, EfSubCategoryLangDal>();
            services.AddScoped<ISubCategoryRepository, EfSubCategoryDal>();
            services.AddScoped<ISupportRepository, EfSupportDal>();
            services.AddScoped<ISupportTypeRepository, EfSupportTypeDal>();
            services.AddScoped<IUserRepository, EfUserDal>();
            services.AddScoped<IPostRepository, EfPostDal>();
            services.AddScoped<IPostTypeRepository, EfPostTypeDal>();

            #endregion AddScoped and AddSingleton and AddTransient

            AuthConfigureServices(services);// Authentication

            SignalRConfigureServices(services);// SignalR

            services
               .AddMvc()
               .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Configuration.GetValue<string>("Redis:Name");
                option.InstanceName = Configuration.GetValue<string>("Redis:Host");
            });

            SwanggerConfigureServices(services);// Swangger
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            LoggerConfigure(env, loggerFactory);// Logger

            AuthConfigure(app);// Authentication

            ExceptionHandlerConfigure(app);// Exception handler

            SignalRConfigure(app);// SignalR

            app.UseMvc();

            SwanggerConfigure(app);// Swangger

            app.Run(async context => await context.Response.WriteAsync("ContestPark Web API v1"));

            DatabaseInitializer.Initialize(app);
        }
    }
}