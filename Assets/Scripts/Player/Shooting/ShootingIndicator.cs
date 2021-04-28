using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class ShootingIndicator : NetworkBehaviour
{
    [SerializeField]
    private int _minimum, _maximum, _current;

    [SerializeField]
    private float _lerpSpeed;

    [SerializeField]
    private Image _firingModeImage, _firingModeTimer;

    [SerializeField]
    private Sprite[] _spriteList;

    private enum _firingMode
    {
        cantShoot = 0,
        singleFire = 1,
        akimbo = 2,
        bazooka = 3
    }

    private _firingMode _lastFiringMode;

    private float _lastTimer = 0, _lastRefTimer = 0;

    private bool _toFill = true;

    private void Start( )
    {
        if (isLocalPlayer)
        {
            _lastFiringMode =_firingMode.cantShoot;
            SetFiringModeImage();
        }
    }

    private void Update( )
    {
        if(isLocalPlayer)
        {
            _lerpSpeed = 10.0f * Time.deltaTime;
        }
    }

    public void DisplayCantShoot( )
    {
        if (_lastFiringMode != _firingMode.cantShoot)
        {
            _lastFiringMode =_firingMode.cantShoot;
            SetFiringModeImage();
        }
    }

    public void DisplaySingleFire( )
    {
        if (_lastFiringMode != _firingMode.singleFire)
        {
            _lastFiringMode =_firingMode.singleFire;
            SetFiringModeImage();
        }
    }

    public void DisplayAkimbo( )
    {
        if (_lastFiringMode != _firingMode.akimbo)
        {
            _lastFiringMode =_firingMode.akimbo;
            SetFiringModeImage();
        }
    }

    public void DisplayBazooka( )
    {
        if (_lastFiringMode != _firingMode.bazooka)
        {
            _lastFiringMode =_firingMode.bazooka;
            SetFiringModeImage();
        }
    }

    private void SetFiringModeImage( )
    {
        _firingModeImage.sprite = _spriteList[(int)_lastFiringMode];
    }

    public void DisplayTimer(float refTimer, float timer)
    {
        // si on a pas de power-up ou qu'on a choppé un nouveau power-up ayant deja un power-up
        if (!_toFill && timer <= 0.0f || _lastRefTimer != refTimer || _lastTimer < timer)
        {
            _toFill = true;
        }

        _lastTimer = timer;
        _lastRefTimer = refTimer;

        float currentOffset = (timer * _maximum / refTimer) - _minimum;
        float maximumOffset = _maximum - _minimum;
        float fillAmount = currentOffset / maximumOffset;

        // si on doit remplir instant et qu'on a un power-up
        if (_toFill && timer > 0.0f)
        {
            _firingModeTimer.fillAmount = maximumOffset;
            _toFill = false;
        }

        // si on décroit
        if (_firingModeTimer.fillAmount > fillAmount)
        {
            _firingModeTimer.fillAmount = Mathf.Lerp(_firingModeTimer.fillAmount, fillAmount, _lerpSpeed);
        }
    }
}
