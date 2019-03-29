namespace ContestPark.DataAccessLayer.Missions
{
    public class Mission35 : IMission
    {
        public Entities.Enums.Missions Mission
        {
            get
            {
                return Entities.Enums.Missions.Mission35;
            }
        }

        /// <summary>
        /// Hoşuna giden bir kategoriyi istediğin sosyal hesabında paylaş.
        /// </summary>
        /// <param name="userId">Kullanıcı id</param>
        /// <returns>Görevi yaptıysa true yapmadıysa false</returns>
        public bool MissionComplete(string userId)
        {
            // Kategoriyi sosyal medya hesabında paylaş
            return true;//Şartı olmadığı için direk true döndü
        }
    }
}