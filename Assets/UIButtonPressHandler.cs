using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isPressed = false;
    public bool IsPressed { get => _isPressed; private set { _isPressed = value; }}

    public void OnPointerDown(PointerEventData data) => IsPressed = true;
    public void OnPointerUp(PointerEventData data) => IsPressed = false;

}
