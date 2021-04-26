﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class TimeManager : NetworkBehaviour
{
    [SyncVar]
    private float _timer;

    [SerializeField]
    private float _gameDuration;

    private TextMeshProUGUI _timerText;

    [ClientCallback]
    private void Start( )
    {
        _timerText = GameObject.Find("GameTimer").GetComponent<TextMeshProUGUI>();
        _timerText.text = string.Empty;
    }

    public override void OnStartServer( )
    {
        _timer = _gameDuration;
    }

    void Update()
    {
        if (isServer)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().StopGame();
            }
        }
        else
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