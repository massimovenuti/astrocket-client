using UnityEngine;
using API.Auth;
using API.Stats;
using System.Linq;
using System.Collections.Generic;

namespace API
{
#if UNITY_EDITOR || UNITY_EDITOR_64
    class API_Tests : MonoBehaviour
    {
        private StatsAPICall _stats = new StatsAPICall();
        private AuthAPICall _auth = new AuthAPICall();

        private void Awake( )
        {
            //AuthApiPostLoginUserTest();
            AuthApiPostCheckUserTokenTest();
            //AuthApiPostAddUserTest();
            //StatsApiGetAllStatsTest();
            //StatsApiGetUserStatsTest();
        }

        public bool AuthApiTests( )
        {
            Debug.Assert(AuthApiPostAddUserTest());
            Debug.Assert(AuthApiPostBanUserTest());
            Debug.Assert(AuthApiPostAddAdminTest());
            Debug.Assert(AuthApiPostAddServerTest());
            Debug.Assert(AuthApiPostLoginUserTest());
            Debug.Assert(AuthApiPostUnbanUserTest());
            Debug.Assert(AuthApiPostRemoveUserTest());
            Debug.Assert(AuthApiPostRemoveAdminTest());
            Debug.Assert(AuthApiPostRemoveServerTest());
            Debug.Assert(AuthApiPostCheckUserTokenTest());
            Debug.Assert(AuthApiPostCheckServerTokenTest());
            return true;
        }

        public bool StatsApiTests( )
        {
            Debug.Assert(StatsApiGetRannkingByScoreTest());
            Debug.Assert(StatsApiDeleteUserStatsTest());
            Debug.Assert(StatsApiPostModifyPlayerStatsTest());
            Debug.Assert(StatsApiGetUserStatsTest());
            return true;
        }

        private bool StatsApiGetRannkingByScoreTest( )
        {
            List<PlayerStats> player_stats1;
            player_stats1 = _stats.GetRannkingByScore();
            Debug.Assert(_stats.ErrorMessage.Status == System.Net.HttpStatusCode.OK);
            return false;
        }

        private bool StatsApiDeleteUserStatsTest( )
        {
            _stats.PostModifyPlayerStats(name: "", token: "", new PlayerStats());
            _stats.DeleteUserStats(name: "", token: "");
            return true;
        }

        private bool StatsApiPostModifyPlayerStatsTest( )
        {
            Debug.Assert(_stats.PostModifyPlayerStats(name: "", token: "", new PlayerStats()));
            return true;
        }

        private bool StatsApiGetUserStatsTest( )
        {
            PlayerStats pstats;
            pstats = _stats.GetUserStats(name: "TestUser");
            Debug.Assert(pstats != null);
            return false;
        }

        private bool StatsApiGetAllStatsTest( )
        {
            List<PlayerStats> lps;
            lps = _stats.GetAllStats();
            //Debug.Assert(lps != null);
            return true;
        }

        private bool AuthApiPostLoginUserTest( )
        {
            UserToken t = _auth.PostLoginUser(new UserLogin() { Name = "NewUser", Password = "newuserpwd" });

            Debug.Log(_auth.ErrorMessage);
            Debug.Assert(t != null);
            Debug.Log($"{t.Token}");
            return true;
        }

        private bool AuthApiPostAddUserTest( )
        {
            UserToken t = _auth.PostAddUser(new UserRegister() { Name = "NewUser", Password = "newuserpwd", Email = "new.user@test.com" });
            Debug.Log(_auth.ErrorMessage);
            Debug.Log($"{t.Token}");
            return true;
        }

        private bool AuthApiPostCheckUserTokenTest( )
        {
            UserRole ur = _auth.PostCheckUserToken(new UserToken { Token = "55848c031c900d182355aa59fade0030416b8450123a19d2cb9647276d8bdfbbc1daa588883fcdae" });
            Debug.Log(_auth.ErrorMessage);
            Debug.Log($"{ur.Name} : {ur.Role}");
            return false;
        }

        private bool AuthApiPostCheckServerTokenTest( )
        {
            ServerName sn = _auth.PostCheckServerToken(token: "");
            Debug.Assert(sn != null);
            return false;
        }

        private bool AuthApiPostBanUserTest( )
        {
            Debug.Assert(_auth.PostBanUser(username: "", adminToken: ""));
            return true;
        }

        private bool AuthApiPostRemoveUserTest( )
        {
            Debug.Assert(_auth.PostRemoveUser(username: "", adminToken: ""));
            return true;
        }

        private bool AuthApiPostAddServerTest( )
        {
            Debug.Assert(_auth.PostAddServer(name: "", user_token: "") != null);
            return false;
        }

        private bool AuthApiPostRemoveServerTest( )
        {
            Debug.Assert(_auth.PostRemoveServer(token: ""));
            return false;
        }

        private bool AuthApiPostAddAdminTest( )
        {
            Debug.Assert(_auth.PostAddAdmin(username: "", token: ""));
            return false;
        }

        private bool AuthApiPostRemoveAdminTest( )
        {
            Debug.Assert(_auth.PostRemoveAdmin(username: "", token: ""));
            return false;
        }

        private bool AuthApiPostUnbanUserTest( )
        {
            Debug.Assert(_auth.PostUnbanUser(username: "", adminToken: ""));
            return false;
        }
    }
#endif
}
