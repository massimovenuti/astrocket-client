using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShootingIndicator : MonoBehaviour
{
    [SerializeField]
    private Image _firingModeImage;

    [SerializeField]
    private Sprite[] _spriteList;

    private int _lastFiringMode;
    private enum _firingMode
    {
        cantShoot = 0,
        singleFire = 1,
        akimbo = 2,
        bazooka = 3
    }

    private void Start( )
    {
        _lastFiringMode = (int)_firingMode.cantShoot;
        SetFiringModeImage();
    }

    public void DisplayCantShoot()
    {
        if (_lastFiringMode != (int)_firingMode.cantShoot)
        {
            _lastFiringMode = (int)_firingMode.cantShoot;
            SetFiringModeImage();
        }
    }

    public void DisplaySingleFire( )
    {
        if (_lastFiringMode != (int)_firingMode.singleFire)
        {
            _lastFiringMode = (int)_firingMode.singleFire;
            SetFiringModeImage();
        }
    }

    public void DisplayAkimbo( )
    {
        if (_lastFiringMode != (int)_firingMode.akimbo)
        {
            _lastFiringMode = (int)_firingMode.akimbo;
            SetFiringModeImage();
        }
    }

    public void DisplayBazooka( )
    {
        if (_lastFiringMode != (int)_firingMode.bazooka)
        {
            _lastFiringMode = (int)_firingMode.bazooka;
            SetFiringModeImage();
        }
    }

    private void SetFiringModeImage( )
    {
        _firingModeImage.sprite = _spriteList[_lastFiringMode];
    }
}
