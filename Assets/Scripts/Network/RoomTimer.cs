using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;


public class RoomTimer : NetworkBehaviour
{
    [SyncVar(hook ="OnTriggerChange")]
    private bool _trigger;

    [SyncVar]
    private float _timer;

    private AsteroidNetworkManager _roomManager;

    private TextMeshProUGUI _timerText;

    [SerializeField]
    private float _roomDuration;

    public override void OnStartServer( )
    {
        _trigger = false;
        _timer = 0;
        _roomManager = GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>();
    }

    public override void OnStartClient( )
    {
        _timerText = GameObject.Find("RoomTimer").GetComponent<TextMeshProUGUI>();
        _timerText.text = string.Empty;
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
                    _roomManager.StartGame();
                    _trigger = false;
                }
            }
            else
            {
                _timerText.text = ((int)_timer).ToString();
            }
        }
    }

    [Server]
    public void SetTimer(float timer)
    {
        _timer = timer;
    }

    [Server]
    public void StopTimer()
    {
        _trigger = false;
        _timer = 0;
    }

    [Server]
    public void StartTimer()
    {
        _trigger = true;
        _timer = _roomDuration;
    }

    private void OnTriggerChange(bool oldValue, bool newValue)
    {
        if (!newValue && _timerText != null)
        {
            _timerText.text = string.Empty;
        }
    }
}
