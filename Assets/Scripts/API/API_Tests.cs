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
            Debug.Log(JsonUtility.ToJson(new UserToken() { Token = "65455645" }));
            Debug.Log(JsonUtility.ToJson(new BanItem() { Token = "65455645", Name = "testuser" }));
            Debug.Log(JsonUtility.ToJson(new UserLogin() { Name = "testuser", Password = "testpass" }));
            Debug.Log(JsonUtility.ToJson(new UserRegister() { Name = "testuser", Password = "testpass", Email = "test@example.com" }));
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
            List<PlayerStats> player_stats1, player_stats2;
            player_stats1 = _stats.GetRannkingByScore();
            Debug.Assert(_stats.ErrorMessage.Status == System.Net.HttpStatusCode.OK);
            player_stats2 = _stats.GetRannkingByScore(1, 20);
            Debug.Assert(_stats.ErrorMessage.Status == System.Net.HttpStatusCode.OK);
            Debug.Assert(player_stats1.Count == 10 && player_stats2.Count == 20);

            player_stats1 = _stats.GetRannking(0);
            Debug.Assert(player_stats1 == null);
            Debug.Assert(_stats.ErrorMessage.Status != System.Net.HttpStatusCode.OK);
            return false;
        }

        private bool StatsApiDeleteUserStatsTest()
        {
            _stats.PostModifyPlayerStats(name: "", token: "");
            _stats.DeleteUserStats(name:"", token:"");
            return true;
        }

        private bool StatsApiPostModifyPlayerStatsTest( )
        {
            Debug.Assert(_stats.PostModifyPlayerStats(name:"",token:""));
            return true;
        }

        private bool StatsApiGetUserStatsTest( )
        {
            PlayerStats pstats;
            pstats = _stats.GetUserStats(name: "");
            Debug.Assert(pstats != null);
            return false;
        }

        private bool StatsApiGetAllStatsTest( )
        {
            List<PlayerStats> lps;
            lps = _stats.GetAllStats();
            Debug.Assert(lps != null);
            return true;
        }

        private bool AuthApiPostLoginUserTest( )
        {
            UserToken t = _auth.PostLoginUser(new UserLogin() { Name = "", Password = "" });
            Debug.Assert(t != null);
            return true;
        }

        private bool AuthApiPostAddUserTest( )
        {
            UserToken t = _auth.PostLoginUser(new UserLogin() { Name = "", Password = "" });
            Debug.Assert(t != null);
            return false;
        }

        private bool AuthApiPostCheckUserTokenTest( )
        {
            UserRole ur = _auth.PostCheckUserToken(new UserToken { Token = "" });
            Debug.Assert(ur != null);
            return false;
        }

        private bool AuthApiPostCheckServerTokenTest( )
        {
            ServerName sn = _auth.PostCheckServerToken(token:"");
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
            Debug.Assert(_auth.PostRemoveServer(token:""));
            return false;
        }

        private bool AuthApiPostAddAdminTest( )
        {
            Debug.Assert(_auth.PostAddAdmin(username: "", token:""));
            return false;
        }

        private bool AuthApiPostRemoveAdminTest( )
        {
            Debug.Assert(_auth.PostRemoveAdmin(username: "", token: ""));
            return false;
        }

        private bool AuthApiPostUnbanUserTest( )
        {
            Debug.Assert(_auth.PostUnbanUser(username: "", adminToken:""));
            return false;
        }
    }
#endif
}
