using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private bool _isPressed = false;
    private bool _isClicked = false;
    private bool _isToggled = false;
    public bool IsPressed { 
        get => _isPressed; 
        private set { 
            _isPressed = value; 
        }
    }

    public bool IsClicked { 
        get {
            if (_isClicked)
            {
                _isClicked = false;
                return true;
            }
            else
                return false;
        }
        private set { 
            _isClicked = value; 
        } 
    }

    public bool IsToggled
    {
        get => _isToggled;
    }

    public void OnPointerDown(PointerEventData data) => IsPressed = true;
    public void OnPointerUp(PointerEventData data) => IsPressed = false;
    public void OnPointerClick(PointerEventData data)
    {
        IsClicked = true;
        _isToggled = !_isToggled;
    }

    private void OnEnable( )
    {
        _isPressed = false;
        _isClicked = false;
        _isToggled = false;
    }
}
