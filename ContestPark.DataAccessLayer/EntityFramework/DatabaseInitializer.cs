using ContestPark.DataAccessLayer.Interfaces.Context;
using ContestPark.DataAccessLayer.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContestPark.DataAccessLayer.Interfaces
{
    public static class DatabaseInitializer
    {
        public static async Task Initialize(/*IServiceProvider services,*/ IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<ContestParkContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ContestParkContext>();
                context.Database.Migrate();

                if (context.Users.Any()) return;

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                #region AspNetRoles

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }

                #endregion AspNetRoles

                #region AspNetUser and AspNetUserRoles

                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [alivelideli].[AspNetUsers] ON");
                var userWitcher = new User()
                {
                    UserName = "witcherfearless",
                    Email = "witcher_fearless@hotmail.com",
                    RegistryDate = DateTime.Now,
                    LastActiveDate = DateTime.Now,
                    Status = true,
                    FullName = "Ali Aldemir",
                    LanguageCode = "tr-TR",
                    ProfilePicturePath = "http://resim.contestpark.com/Resimler/UyeFotograflari/e203b4e086d147a19c8eff8878cebce90030551742016007f35bd761e4b14b75a393eb3f33c3c.jpg"
                };

                IdentityResult result = await userManager.CreateAsync(userWitcher, "19931993");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(userWitcher, new string[] { "User" });
                }
                var userElifoz = new User()
                {
                    UserName = "elifoz",
                    Email = "aldemirali93@gmail.com",
                    RegistryDate = DateTime.Now,
                    LastActiveDate = DateTime.Now,
                    Status = true,
                    FullName = "Elif Öz",
                    LanguageCode = "tr-TR",
                    ProfilePicturePath = "http://resim.contestpark.com/Resimler/UyeFotograflari/8371f41cff5b407cbbc0fec313e8cefc2238512542016007f35bd761e4b14b75a393eb3f33c3c.jpg"
                };
                string userIdWitcher = userWitcher.Id, userIdElif = userElifoz.Id;

                IdentityResult result1 = await userManager.CreateAsync(userElifoz, "19931993");
                if (result1.Succeeded)
                {
                    await userManager.AddToRolesAsync(userElifoz, new string[] { "User" });
                }
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [alivelideli].[AspNetUsers] OFF");

                #endregion AspNetUser and AspNetUserRoles

                await context.Languages.AddRangeAsync(
                            new Language { LanguageId = 1, LongName = "Türkçe", ShortName = "tr-TR" },
                            new Language { LanguageId = 2, LongName = "İngilizce", ShortName = "en-US" }
                    );

                context.SaveChanges();

                await context.Boosts.AddRangeAsync(
                            new Boost { Name = "boostPage.fiftyFifty", Gold = 150, PicturePath = "assets/images/fiftyfifty.png", Visibility = true },
                            new Boost { Name = "boostPage.dualChance", Gold = 150, PicturePath = "assets/images/doubleanswer.png", Visibility = true },
                            new Boost { Name = "boostPage.extra10", Gold = 150, PicturePath = "assets/images/addtime.png", Visibility = true },
                            new Boost { Name = "boostPage.freezeTime", Gold = 150, PicturePath = "assets/images/freezetime.png", Visibility = true },
                            new Boost { Name = "boostPage.2x", Gold = 150, PicturePath = "assets/images/2xscore.png", Visibility = true },
                            new Boost { Name = "boostPage.audience", Gold = 150, PicturePath = "assets/images/Asktheaudience.png", Visibility = true }
                    );

                context.SaveChanges();

                await context.Categories.AddRangeAsync(
                            new Category { PicturePath = "http://resim.contestpark.com/CategoryImages/BilgiYarismasi.png", Visibility = true, Order = 3 },
                            //fake
                            new Category { PicturePath = "http://resim.contestpark.com/CategoryImages/BilgiYarismasi.png", Visibility = true, Order = 3 },
                            new Category { PicturePath = "http://resim.contestpark.com/CategoryImages/BilgiYarismasi.png", Visibility = true, Order = 3 },
                            new Category { PicturePath = "http://resim.contestpark.com/CategoryImages/BilgiYarismasi.png", Visibility = true, Order = 3 }
                    //new Category { PicturePath = "http://resim.contestpark.com/CategoryImages/MuzikYarismasi.jpg", Visibility = false, Order = 4 },
                    //new Category { PicturePath = "http://resim.contestpark.com/CategoryImages/Flags.png", Visibility = false, Order = 5 },
                    //       new Category { PicturePath = "http://resim.contestpark.com/CategoryImages/AppsLogos.png", Visibility = true, Order = 2 }
                    //new Category { PicturePath = "", Visibility = true, Order = 1 }
                    );

                context.SaveChanges();

                await context.ContestDates.AddAsync(new ContestDate { StartDate = DateTime.Now, FinishDate = DateTime.Now.AddYears(3) });

                context.SaveChanges();

                await context.Cps.AddAsync(new Cp { UserId = userIdWitcher, CpAmount = 9999999 });
                context.SaveChanges();
                await context.Cps.AddAsync(new Cp { UserId = userIdElif, CpAmount = 9999999 });

                context.SaveChanges();

                await context.Follows.AddRangeAsync(
                            new Follow { FollowedUserId = userIdWitcher, FollowUpUserId = userIdElif },
                            new Follow { FollowedUserId = userIdElif, FollowUpUserId = userIdWitcher }
                    );

                context.SaveChanges();

                await context.Missions.AddRangeAsync(
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, },
                            new Mission { Gold = 50, MissionOpeningImage = "Images/lock.png", MissionCloseingImage = "Images/lock.png", Visibility = true, }
                    );

                context.SaveChanges();

                await context.MissionLangs.AddRangeAsync(
                            new MissionLang { MissionName = "Hoş Geldin", MissionDescription = "İlk düellonu tamamlamalısın.", LanguageId = 1, MissionId = 1 },
                            new MissionLang { MissionName = "Welcome", MissionDescription = "Finish your first duel.", LanguageId = 2, MissionId = 1, },
                            new MissionLang { MissionName = "Kanım Kaynadı", MissionDescription = "20 düello tamamlamalısın.", LanguageId = 1, MissionId = 2 },
                            new MissionLang { MissionName = "Warm-Up", MissionDescription = "Finish 20 duel.", LanguageId = 2, MissionId = 2 },
                            new MissionLang { MissionName = "Sardı Sarmaladı", MissionDescription = "50 düello tamamlamalısın..", LanguageId = 1, MissionId = 3 },
                            new MissionLang { MissionName = "Wrapped Up", MissionDescription = "Finish 50 duel.", LanguageId = 2, MissionId = 3 }
                    );

                context.SaveChanges();

                await context.NotificationTypes.AddRangeAsync(
                            new NotificationType { IsActive = true },
                            new NotificationType { IsActive = true },
                            new NotificationType { IsActive = true },
                            new NotificationType { IsActive = true },
                            new NotificationType { IsActive = true },
                            new NotificationType { IsActive = true },
                            new NotificationType { IsActive = true }
                    );

                context.SaveChanges();

                await context.NotificationTypeLangs.AddRangeAsync(
                            new NotificationTypeLang { NotificationTypeId = 1, LanguageId = 1, NotificationName = "{kullaniciadi} seninle <b>{yarisma}</b> yarışmasında <b>düello</b> yaptı. Ona karşı koy!" },
                            new NotificationTypeLang { NotificationTypeId = 1, LanguageId = 2, NotificationName = "<b>{kullaniciadi}</b> to <b>duel</b> you in the <b>{yarisma}</b> contest. resist!" },
                            new NotificationTypeLang { NotificationTypeId = 2, LanguageId = 1, NotificationName = "<b>{kullaniciadi}</b> seni <b>takip</b> ediyor." },
                            new NotificationTypeLang { NotificationTypeId = 2, LanguageId = 2, NotificationName = "<b>{kullaniciadi}</b> followed you." },
                            new NotificationTypeLang { NotificationTypeId = 3, LanguageId = 1, NotificationName = "Senin bağlantını <b>beğendi.</b>" },
                            new NotificationTypeLang { NotificationTypeId = 3, LanguageId = 2, NotificationName = "<b>{kullaniciadi}</b> liked your post." },
                            new NotificationTypeLang { NotificationTypeId = 4, LanguageId = 1, NotificationName = "Senin bağlantına <b>yorum</b> yaptı." },
                            new NotificationTypeLang { NotificationTypeId = 4, LanguageId = 2, NotificationName = "<b>{kullaniciadi}</b> commented on your post." },
                            new NotificationTypeLang { NotificationTypeId = 5, LanguageId = 1, NotificationName = "<b>{yarisma}</b> yarışmasında düellona <b>karşı koydu!</b>" },
                            new NotificationTypeLang { NotificationTypeId = 5, LanguageId = 2, NotificationName = "<b>{kullaniciadi}</b> resisted a duel song contest." },
                            new NotificationTypeLang { NotificationTypeId = 6, LanguageId = 1, NotificationName = "<b>{kullaniciadi} {yarisma}</b> yarışmasında kaybetmeyi seçti." },
                            new NotificationTypeLang { NotificationTypeId = 6, LanguageId = 2, NotificationName = "<b>{kullaniciadi}</b> choose to lose in the song contest." }
                    );

                context.SaveChanges();

                await context.Notifications.AddRangeAsync(
                            new Notification { NotificationTypeId = 1, WhonId = userIdWitcher, WhoId = userIdElif, SubCategoryId = 1, NotificationDate = DateTime.Now, Link = "1", Status = true },
                            new Notification { NotificationTypeId = 2, WhonId = userIdWitcher, WhoId = userIdElif, SubCategoryId = 0, NotificationDate = DateTime.Now, Link = "#", Status = true },
                            new Notification { NotificationTypeId = 3, WhonId = userIdWitcher, WhoId = userIdElif, SubCategoryId = 0, NotificationDate = DateTime.Now, Link = "1", Status = true },
                            new Notification { NotificationTypeId = 4, WhonId = userIdWitcher, WhoId = userIdElif, SubCategoryId = 0, NotificationDate = DateTime.Now, Link = "1", Status = true },
                            new Notification { NotificationTypeId = 5, WhonId = userIdWitcher, WhoId = userIdElif, SubCategoryId = 0, NotificationDate = DateTime.Now, Link = "1", Status = true },
                            new Notification { NotificationTypeId = 5, WhonId = userIdWitcher, WhoId = userIdElif, SubCategoryId = 0, NotificationDate = DateTime.Now, Link = "1", Status = true }
                    );

                context.SaveChanges();

                await context.CategoryLangs.AddRangeAsync(
                            new CategoryLang { Name = "Karışık", CategoryId = 1, LanguageId = 1 },
                            new CategoryLang { Name = "Mixed", CategoryId = 1, LanguageId = 2 },
                            new CategoryLang { Name = "Şarkılar", CategoryId = 2, LanguageId = 1 },
                            new CategoryLang { Name = "Songs", CategoryId = 2, LanguageId = 2 },
                            new CategoryLang { Name = "Bayraklar", CategoryId = 3, LanguageId = 1 },
                            new CategoryLang { Name = "Flags", CategoryId = 3, LanguageId = 2 },
                            new CategoryLang { Name = "Uygulama Logoları", CategoryId = 4, LanguageId = 1 },
                            new CategoryLang { Name = "App Logos", CategoryId = 4, LanguageId = 2 }
                            //new CategoryLang { CategoryName = "Futbol", CategoryId = 5, LanguageId = 1 },
                            //new CategoryLang { CategoryName = "Football", CategoryId = 5, LanguageId = 2 }
                    );

                context.SaveChanges();

                #region Fake sub categories

                for (int j = 1; j <= 4; j++)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        SubCategory subCategory = new SubCategory { CategoryId = j, Visibility = true, PictuePath = "http://resim.contestpark.com/CategoryImages/UlkeBayraklari.png", Order = 1, Price = i };
                        await context.SubCategories.AddRangeAsync(
                                   subCategory
                            //new SubCategory { CategoryId = 1, Visibility = true, PictuePath = "http://resim.contestpark.com/CategoryImages/TVCharacters.png", Order = 2, Price = 5000 },
                            //new SubCategory { CategoryId = 1, Visibility = true, PictuePath = "http://resim.contestpark.com/CategoryImages/CsGo.png", Order = 3, Price = 10000 },

                            //new SubCategory { CategoryId = 2, Visibility = true, PictuePath = "http://resim.contestpark.com/CategoryImages/UlkeBayraklari.png", Order = 2, Price = 10000 },

                            //new SubCategory { CategoryId = 3, Visibility = true, PictuePath = "http://resim.contestpark.com/CategoryImages/Fotbolcular.jpg", Order = 1, Price = 2000 },
                            //new SubCategory { CategoryId = 3, Visibility = true, PictuePath = "http://resim.contestpark.com/CategoryImages/Stadyum.png", Order = 2, Price = 0 },
                            //new SubCategory { CategoryId = 3, Visibility = true, PictuePath = "http://resim.contestpark.com/CategoryImages/Antrenor.jpg", Order = 3, Price = 5000 },
                            //new SubCategory { CategoryId = 3, Visibility = true, PictuePath = "http://resim.contestpark.com/CategoryImages/Hakem.jpg", Order = 4, Price = 3000 }
                            );

                        context.SaveChanges();

                        await context.SubCategoryLangs.AddRangeAsync(
                                    new SubCategoryLang { SubCategoryId = subCategory.SubCategoryId, LanguageId = 1, SubCategoryName = "Ülke Bayrakları" },
                                    new SubCategoryLang { SubCategoryId = subCategory.SubCategoryId, LanguageId = 2, SubCategoryName = "Country Flags" }
                            );
                    }
                }

                #endregion Fake sub categories

                await context.FollowCategories.AddAsync(new FollowCategory { SubCategoryId = 1, UserId = userIdWitcher });

                context.SaveChanges();

                await context.OpenSubCategories.AddAsync(new OpenSubCategory { SubCategoryId = 1, UserId = userIdWitcher });
                context.SaveChanges();

                await context.Questions.AddRangeAsync(
                    new Question
                    {
                        AnswerType = Entities.Enums.AnswerTypes.Text,
                        IsActive = true,
                        SubCategoryId = 1,
                        QuestionType = Entities.Enums.QuestionTypes.Image,
                        Link = "http://resim.contestpark.com/Flags/fc9ff6a2-a9b4-4a6d-8926-558ff6f879f5.png",
                        QuestionLangs =
                        {
                             new QuestionLang{ Questions = "Bu bayrak hangi devlete ait?",  Answer="ABD Virgin Adaları", Stylish1="Slovenya", Stylish2 ="Barbados", Stylish3="Mikronezya", LanguageId = 1},
                             new QuestionLang{ Questions = "This flag, which belongs to the state?",  Answer="US Virgin Islands", Stylish1="Slovenia", Stylish2 ="barbados", Stylish3="Micronesia", LanguageId = 2},
                        }
                    },
                    new Question
                    {
                        AnswerType = Entities.Enums.AnswerTypes.Text,
                        IsActive = true,
                        SubCategoryId = 1,
                        QuestionType = Entities.Enums.QuestionTypes.Image,
                        Link = "http://resim.contestpark.com/Flags/b27d31e6-93e5-42f3-ac36-6aedf1553a55.png",
                        QuestionLangs =
                        {
                             new QuestionLang{ Questions = "Bu bayrak hangi devlete ait?",  Answer="Afganistan", Stylish1="Seysel Adaları", Stylish2 ="Myanmar", Stylish3="Cibuti", LanguageId = 1},
                             new QuestionLang{ Questions = "This flag, which belongs to the state?",  Answer="Afghanistan", Stylish1="Seychelles", Stylish2 ="Myanmar", Stylish3="Djibouti", LanguageId = 2},
                        }
                    },
                    new Question
                    {
                        AnswerType = Entities.Enums.AnswerTypes.Text,
                        IsActive = true,
                        SubCategoryId = 1,
                        QuestionType = Entities.Enums.QuestionTypes.Image,
                        Link = "http://resim.contestpark.com/Flags/1124f6a4-f514-4a29-aeb4-a0f96e0ee803.png",
                        QuestionLangs =
                        {
                             new QuestionLang{ Questions = "Bu bayrak hangi devlete ait?",  Answer="Almanya", Stylish1="Antigua & Barbuda", Stylish2 ="Tunus", Stylish3="Macaristan", LanguageId = 1},
                             new QuestionLang{ Questions = "This flag, which belongs to the state?",  Answer="Germany", Stylish1="Antigua", Stylish2 ="Tunisian", Stylish3="Hungary", LanguageId = 2},
                        }
                    },
                    new Question
                    {
                        AnswerType = Entities.Enums.AnswerTypes.Text,
                        IsActive = true,
                        SubCategoryId = 1,
                        QuestionType = Entities.Enums.QuestionTypes.Image,
                        Link = "http://resim.contestpark.com/Flags/e7d2d397-c46b-430e-85ae-5fccb1296f12.png",
                        QuestionLangs =
                        {
                             new QuestionLang{ Questions = "Bu bayrak hangi devlete ait?",  Answer="Amerika Birleşik Devletleri", Stylish1="Amerikan Samoa", Stylish2 ="Nijer", Stylish3="Guyana", LanguageId = 1},
                             new QuestionLang{ Questions = "This flag, which belongs to the state?",  Answer="United States of America", Stylish1="American Samoa", Stylish2 ="Niger", Stylish3="Guyana", LanguageId = 2},
                        }
                    },
                    new Question
                    {
                        AnswerType = Entities.Enums.AnswerTypes.Text,
                        IsActive = true,
                        SubCategoryId = 1,
                        QuestionType = Entities.Enums.QuestionTypes.Image,
                        Link = "http://resim.contestpark.com/Flags/7bfcc525-7476-4aea-b1d5-28b6c22616f5.png",
                        QuestionLangs =
                        {
                             new QuestionLang{ Questions = "Bu bayrak hangi devlete ait?",  Answer="Amerikan Samoa", Stylish1="Birleşik Arap Emirlikleri", Stylish2 ="Barbados", Stylish3="Filipinler", LanguageId = 1},
                             new QuestionLang{ Questions = "This flag, which belongs to the state?",  Answer="American Samoa", Stylish1="United Arab Emirates", Stylish2 ="barbados", Stylish3="Philippines", LanguageId = 2},
                        }
                    },
                    new Question
                    {
                        AnswerType = Entities.Enums.AnswerTypes.Text,
                        IsActive = true,
                        SubCategoryId = 1,
                        QuestionType = Entities.Enums.QuestionTypes.Image,
                        Link = "http://resim.contestpark.com/Flags/7f0d9cbc-e18e-4acd-826e-73e6f5e9d5ae.png",
                        QuestionLangs =
                        {
                             new QuestionLang{ Questions = "Bu bayrak hangi devlete ait?",  Answer="Andorra", Stylish1="Nauru", Stylish2 ="Avustralya", Stylish3="Saint Pierre Miquelon", LanguageId = 1},
                             new QuestionLang{ Questions = "This flag, which belongs to the state?",  Answer="Andorra", Stylish1="Nauru", Stylish2 ="Australia", Stylish3="Saint Pierre Miquelon", LanguageId = 2},
                        }
                    },
                    new Question
                    {
                        AnswerType = Entities.Enums.AnswerTypes.Text,
                        IsActive = true,
                        SubCategoryId = 1,
                        QuestionType = Entities.Enums.QuestionTypes.Image,
                        Link = "http://resim.contestpark.com/Flags/ef758d09-91ec-4bb5-b568-3cdf8fbe0c38.png",
                        QuestionLangs =
                        {
                             new QuestionLang{ Questions = "Bu bayrak hangi devlete ait?",  Answer="Angola", Stylish1="Kolombiya", Stylish2 ="Cayman Adaları", Stylish3="Uganda", LanguageId = 1},
                             new QuestionLang{ Questions = "This flag, which belongs to the state?",  Answer="Angola", Stylish1="Colombia", Stylish2 ="Cayman Islands", Stylish3="Uganda", LanguageId = 2},
                        }
                    });

                await context.SettingTypes.AddAsync(new SettingType { SettingName = "Dil seçimi" });

                await context.PostTypes.AddRangeAsync(
                            new PostType { IsActive = true },
                            new PostType { IsActive = true },
                            new PostType { IsActive = true },
                            new PostType { IsActive = true }
                    );

                context.SaveChanges();

                await context.PostTypeLangs.AddRangeAsync(
                            new PostTypeLang { Description = "<b>{yarisma}</b> yarışmasında düello yaptı.", LanguageId = 1, PostTypeId = 1 },
                            new PostTypeLang { Description = "<b>{kullaniciadi}</b> duel in the <b>{yarisma}</b> contest.", LanguageId = 2, PostTypeId = 1 },

                            new PostTypeLang { Description = "<b>{kullaniciadi}</b> isimli kullanıcıyı takip etti.", LanguageId = 1, PostTypeId = 2 },
                            new PostTypeLang { Description = "<b>{kullaniciadi}</b> is followed users.", LanguageId = 2, PostTypeId = 2 },

                            new PostTypeLang { Description = "<b>Profil</b> resmini değiştirdi.", LanguageId = 1, PostTypeId = 3 },
                            new PostTypeLang { Description = "It has changed the <b>profile</b> picture.", LanguageId = 2, PostTypeId = 3 },

                            new PostTypeLang { Description = "<b>Kapak</b> resmini değiştirdi.", LanguageId = 1, PostTypeId = 4 },
                            new PostTypeLang { Description = "It has changed the <b>cover</b> picture.", LanguageId = 2, PostTypeId = 4 }
                    );

                context.SaveChanges();
            }
            Console.WriteLine("Database initializer success!!!");
        }
    }
}