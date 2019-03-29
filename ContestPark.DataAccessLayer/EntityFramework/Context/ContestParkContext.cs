using ContestPark.DataAccessLayer.Tables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace ContestPark.DataAccessLayer.Interfaces.Context
{
    public class ContestParkContext : IdentityDbContext<User>, IDesignTimeDbContextFactory<ContestParkContext>// DbContext
    {
        #region Properties

        public DbSet<AskedQuestion> AskedQuestions { get; set; }
        public DbSet<Boost> Boosts { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatBlock> ChatBlocks { get; set; }
        public DbSet<ChatReply> ChatReplies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CompletedMission> CompletedMissions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLang> CategoryLangs { get; set; }
        public DbSet<ContestDate> ContestDates { get; set; }
        public DbSet<Cp> Cps { get; set; }
        public DbSet<CpInfo> CpInfoes { get; set; }
        public DbSet<Duel> Duels { get; set; }
        public DbSet<DuelInfo> DuelInfoes { get; set; }
        public DbSet<FollowCategory> FollowCategories { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationTypeLang> NotificationTypeLangs { get; set; }
        public DbSet<OpenSubCategory> OpenSubCategories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionLang> QuestionLangs { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SettingType> SettingTypes { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<SubCategoryLang> SubCategoryLangs { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<SupportType> SupportTypes { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<MissionLang> MissionLangs { get; set; }

        //    public DbSet<User> Users { get; set; }
        public DbSet<SupportTypeLang> SupportTypeLangs { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<PostTypeLang> PostTypeLangs { get; set; }

        #endregion Properties

        public ContestParkContext()
        {
        }

        public ContestParkContext(DbContextOptions<ContestParkContext> options) : base(options)
        {
            //Script-Migration -From 20171113153435_First -To 20171113153435_First
        }

        #region Migrations

        public ContestParkContext CreateDbContext(string[] args)
        {
            // TODO: constr appsettings gelmeli
            var connectionString = "Data Source=mssql04.turhost.com;Initial Catalog=ContestParkTestDb;MultipleActiveResultSets=true;Persist Security Info=True;User ID=alivelideli;Password=Fsw4y42*";
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(
                $"{nameof(connectionString)} is null or empty.",
                nameof(connectionString));

            var optionsBuilder = new DbContextOptionsBuilder<ContestParkContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ContestParkContext(optionsBuilder.Options);
        }

        #endregion Migrations

        #region Override methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //    Bire çok ilişkiler
            modelBuilder
                .Entity<Chat>()
                .HasOne(t => t.SenderUser)
                .WithMany(t => t.Senders)
                .HasForeignKey(d => d.SenderId);

            modelBuilder
                .Entity<Chat>()
                .HasOne(t => t.ReceiverUser)
                .WithMany(t => t.Receivers)
                .HasForeignKey(d => d.ReceiverId);

            modelBuilder
                .Entity<Duel>()
                .HasOne(t => t.FounderUser)
                .WithMany(t => t.Founders)
                .HasForeignKey(d => d.FounderUserId);

            modelBuilder
                .Entity<Duel>()
                .HasOne(t => t.CompetitorUser)
                .WithMany(t => t.Competitors)
                .HasForeignKey(d => d.CompetitorUserId);

            modelBuilder
                .Entity<Follow>()
                .HasOne(t => t.FollowedUser)
                .WithMany(t => t.FollowedUsers)
                .HasForeignKey(d => d.FollowedUserId);

            modelBuilder
                .Entity<Follow>()
                .HasOne(t => t.FollowUpUser)
                .WithMany(t => t.FollowUpUsers)
                .HasForeignKey(d => d.FollowUpUserId);

            modelBuilder
                .Entity<ChatBlock>()
                .HasOne(t => t.WhoUser)
                .WithMany(t => t.Whos)
                .HasForeignKey(d => d.WhoId);

            modelBuilder
                .Entity<ChatBlock>()
                .HasOne(t => t.WhonUser)
                .WithMany(t => t.Whons)
                .HasForeignKey(d => d.WhonId);

            modelBuilder
                .Entity<Notification>()
                .HasOne(t => t.Whon)
                .WithMany(t => t.NotificationWhons)
                .HasForeignKey(d => d.WhonId);

            modelBuilder
                .Entity<Notification>()
                .HasOne(t => t.Who)
                .WithMany(t => t.NotificationWhos)
                .HasForeignKey(d => d.WhoId);

            //Bire bir ilişkiler

            // Delete Behavior
            modelBuilder.Entity<DuelInfo>()
                .HasOne(p => p.Question)
                .WithMany(b => b.DuelInfoes)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Score>()
                .HasOne(p => p.SubCategory)
                .WithMany(b => b.Scores)
                .OnDelete(DeleteBehavior.Restrict);

            // Key
            modelBuilder.Entity<Language>()
                .HasKey(p => p.LanguageId);

            base.OnModelCreating(modelBuilder);
        }

        #endregion Override methods
    }
}