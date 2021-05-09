using System.Collections;
using System.Collections.Generic;
using API;
using API.Auth;
using API.Stats;
using TMPro;
using UnityEngine;

public class StatsScreenManager : ScreenManager
{
    private PlayerStats _currentPlayerStats;
    public override void Start()
    {
        base.Start();
        StatsAPICall stats = new StatsAPICall();
        AuthAPICall auth = new AuthAPICall();
        UserRole info = auth.PostCheckUserToken(SharedInfo.userToken);
        _currentPlayerStats = stats.GetUserStats(name:info.Name);
        TMP_Text username = transform.Find("StatsUI/PlayerTable/UserInfoHeader/UsernameValue").GetComponent<TMP_Text>();
        username.text = info.Name;

        TMP_Text[] texts = transform.Find("StatsUI/StatsTable").GetComponentsInChildren<TMP_Text>();
        foreach(TMP_Text text in texts)
        {
            switch (text.gameObject.name)
            {
                case "Title":
                    break;
                case "Points":
                    text.text = $"You are cumulating a grand total of {_currentPlayerStats.nbPoints} !";
                    break;
                case "Kills":
                    text.text = $"You destroyed {_currentPlayerStats.nbKills} ennemy players";
                    break;
                case "Asteroids":
                    text.text = $"You destroyed {_currentPlayerStats.nbAsteroids} !";
                    break;
                case "Deaths":
                    text.text = $"You died a grand total of {_currentPlayerStats.nbDeaths} times";
                    break;
                case "PowerUps":
                    text.text = $"You grabbed a total of {_currentPlayerStats.nbPowerUps} Power Ups";
                    break;
                case "Games":
                    text.text = $"You played {_currentPlayerStats.nbGames} with us";
                    break;
                case "Wins":
                    text.text = $"You won {_currentPlayerStats.nbWins} during the time you played with us";
                    break;
                case "MaxKills":
                    text.text = $"You managed to destroy {_currentPlayerStats.maxKills} ennemy ships in one game";
                    break;
                case "MaxPoints":
                    text.text = $"You managed to cumulate a record of {_currentPlayerStats.maxPoints} in one game";
                    break;
                case "MaxPowerUps":
                    text.text = $"You managed to grab a maximum of {_currentPlayerStats.maxPowerUps} in one game";
                    break;
                case "MaxDeaths":
                    text.text = $"Your maximum death count in a game is : {_currentPlayerStats.maxDeaths}";
                    break;
                default:
                    text.text = "Defaulted in switch, check spelling";
                    break;
            }


        }
    }

    void Update()
    {
        checkBackKey();
    }
}
