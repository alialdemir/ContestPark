using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ContestPark.DataAccessLayer.Dapper.Repositories
{
    public class AskedQuestionDapperRepository : DapperRepositoryBase<AskedQuestion>, IAskedQuestionRepository
    {
        #region Constructors

        public AskedQuestionDapperRepository(IConfiguration configuration) : base(configuration)
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
            string sql = "delete from AskedQuestions where UserId=@UserId and SubCategoryId=@SubCategoryId";
            int rowCount = await Connection.ExecuteAsync(sql, new { UserId = userId, SubCategoryId = subCategoryId });
            if (rowCount <= 0) Check.BadStatus("Sorulan sorular silinemedi");// ingilizce dil desteği ekle buraya
        }

        #endregion Methods
    }
}