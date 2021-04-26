using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShootingIndicator : MonoBehaviour
{
    [SerializeField]
    private Image _firingModeImage;

    [SerializeField]
    private Sprite _singleFireSprite;

    [SerializeField]
    private Sprite _akimboSprite;

    [SerializeField]
    private Sprite _bazookaSprite;

    private string _lastFiringMode = "Can't Shoot";

    public void DisplayCantShoot()
    {
        Debug.Log("Can't Shoot");
    }

    public void DisplaySingleFire( )
    {
        if (!string.Equals(_lastFiringMode, "Single Fire"))
        {
            _firingModeImage.sprite = _singleFireSprite;
            _lastFiringMode = "Single Fire";
        }
    }

    public void DisplayAkimbo( )
    {
        if (!string.Equals(_lastFiringMode, "Akimbo"))
        {
            _firingModeImage.sprite = _akimboSprite;
            _lastFiringMode = "Akimbo";
        }
    }

    public void DisplayBazooka( )
    {
        if(!string.Equals(_lastFiringMode, "Bazooka"))
        {
            _firingModeImage.sprite = _bazookaSprite;
            _lastFiringMode = "Bazooka";
        }
    }
}
