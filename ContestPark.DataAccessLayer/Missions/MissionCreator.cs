using ContestPark.DataAccessLayer.Interfaces;
using System.Collections.Generic;

namespace ContestPark.DataAccessLayer.Missions
{
    public class MissionCreator : IMissionCreator
    {
        private readonly IDbFactory _dbFactory;

        public MissionCreator(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IMission[] MissionFactory(params Entities.Enums.Missions[] missions)
        {
            List<IMission> result = new List<IMission>();
            foreach (Entities.Enums.Missions mission in missions)
            {
                switch (mission)
                {
                    case Entities.Enums.Missions.Mission1:
                        result.Add(new Mission1(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission2:
                        result.Add(new Mission2(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission3:
                        result.Add(new Mission3(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission4:
                        result.Add(new Mission4(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission5:
                        result.Add(new Mission5(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission6:
                        result.Add(new Mission6(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission7:
                        result.Add(new Mission7(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission8:
                        result.Add(new Mission8(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission9:
                        result.Add(new Mission9(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission10:
                        result.Add(new Mission10(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission11:
                        result.Add(new Mission11(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission12:
                        result.Add(new Mission12(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission13:
                        result.Add(new Mission13(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission14:
                        result.Add(new Mission14(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission15:
                        result.Add(new Mission15(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission16:
                        result.Add(new Mission16(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission17:
                        result.Add(new Mission17(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission18:
                        result.Add(new Mission18(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission19:
                        result.Add(new Mission19(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission20:
                        result.Add(new Mission20(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission21:
                        result.Add(new Mission21(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission22:
                        result.Add(new Mission22(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission23:
                        result.Add(new Mission23(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission24:
                        result.Add(new Mission24(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission25:
                        result.Add(new Mission25(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission26:
                        result.Add(new Mission26(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission27:
                        result.Add(new Mission27(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission28:
                        result.Add(new Mission28(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission29:
                        result.Add(new Mission29(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission30:
                        result.Add(new Mission30(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission31:
                        result.Add(new Mission31(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission32:
                        result.Add(new Mission32(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission33:
                        result.Add(new Mission33(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission34:
                        result.Add(new Mission34(_dbFactory));
                        break;

                    case Entities.Enums.Missions.Mission35:
                        result.Add(new Mission35());
                        break;

                    case Entities.Enums.Missions.Mission36:
                        result.Add(new Mission36(_dbFactory));
                        break;

                    default:
                        break;
                }
            }
            return result.ToArray();
        }
    }
}