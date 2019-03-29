using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ContestPark.DataAccessLayer.DataAccess
{
    public class EfAskedQuestionDal : EfEntityRepositoryBase<AskedQuestion>, IAskedQuestionRepository
    {
        #region Constructors

        public EfAskedQuestionDal(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Sorulan soruları silme
        /// </summary>
        /// <param name="userId">Kullanıcı Id</param>
        /// <param name="subCategoryId">Alt kategori Id</param>
        public async Task DeleteAsync(string userId, int subCategoryId)//Soru bittiğinde kullanıcı id'sine göre tablodaki kayıtları sildik
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            if (subCategoryId <= 0)
                throw new ArgumentNullException(nameof(userId));

            int rowCount = await DbContext.Database.ExecuteSqlCommandAsync($"delete from AskedQuestions where UserId={userId} and SubCategoryId={subCategoryId}", default(System.Threading.CancellationToken));
            if (rowCount <= 0) BadStatus("Sorulan sorular silinemedi");// ingilizce dil desteği ekle buraya
        }

        #endregion Methods
    }
}