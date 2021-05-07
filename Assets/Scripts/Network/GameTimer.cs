using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class GameTimer : NetworkBehaviour
{
    [SyncVar]
    private float _timer;

    [SyncVar(hook ="OnEndGameChange")]
    private bool _endGame;

    [SyncVar]
    private bool _trigger;

    [SerializeField]
    private float _gameDuration, endGameDuration;

    private TextMeshProUGUI _timerText;

    private ScoreTabManager _tabManager;

    private AsteroidNetworkManager _roomManager;


    public override void OnStartServer( )
    {
        _timer = _gameDuration;
        _endGame = false;
        _trigger = true;
        _roomManager = GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>();
    }

    public override void OnStartClient( )
    {
        _timerText = GameObject.Find("GameTimer").GetComponent<TextMeshProUGUI>();
        _timerText.text = string.Empty;
        _tabManager = GameObject.Find("ScoreCanvas").GetComponent<ScoreTabManager>();
    }

    void Update( )
    {
        if (_trigger)
        {
            if (isServer)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    if (!_endGame)
                    {
                        _endGame = true;
                        _timer = endGameDuration;
                        SetPlayersActive(false);
                    }
                    else
                    {
                        _roomManager.StopGame();
                        _trigger = false;
                    }
                }
            }
            else
            {
                if (!_endGame)
                {
                    int min = (int)(_timer / 60);
                    int sec = (int)(_timer % 60);
                    _timerText.text = min.ToString() + ":";
                    if (sec < 10)
                        _timerText.text += "0";
                    _timerText.text += sec.ToString();
                }
            }
        }
    }

    private void OnEndGameChange(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            _timerText.text = string.Empty;
            _tabManager.SetEndGame(true);
            _tabManager.DisplayScore();
            SetPlayersActive(false);
        }
    }

    [Server]
    public void SetTimer(float timer)
    {
        _timer = timer;
    }

    private void SetPlayersActive(bool active)
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            go.SetActive(active);
        }
    }
}
