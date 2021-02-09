using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// This script is adapted from the Luminosity.IO.Examples namespace. 
/// You can the orginal version of the code in the Examples section on the InputManager's GitHub
/// </summary>

[RequireComponent(typeof(Image))]
public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum AxisOption
    {
        Both, OnlyHorizontal, OnlyVertical
    }

    [SerializeField]
    private AxisOption m_axesToUse = AxisOption.Both;
    [SerializeField]
    private Vector2 m_sensitivity = Vector2.one;
    [SerializeField]
    private BindingReference m_horizontalAxisBinding = null;
    [SerializeField]
    private BindingReference m_verticalAxisBinding = null;

    private RectTransform m_transform;
    private Vector2 m_pointerPos;
    private float m_horizontal;
    private float m_vertical;
    private bool m_isPointerDown;

    private void Awake( )
    {
        m_transform = GetComponent<RectTransform>();
        m_isPointerDown = false;
        m_horizontal = 0.0f;
        m_vertical = 0.0f;
        ResetAxisValues();
        InputManager.RemoteUpdate += OnRemoteInputUpdate;
    }

    private void OnDestroy( )
    {
        InputManager.RemoteUpdate -= OnRemoteInputUpdate;
    }

    private void OnRemoteInputUpdate(PlayerID playerID)
    {
        if (playerID == PlayerID.One)
        {
            SetHorizontalAxis(m_horizontal);
            SetVerticalAxis(m_vertical);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_isPointerDown = true;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_transform, eventData.position, eventData.pressEventCamera, out m_pointerPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_isPointerDown)
        {
            Vector2 lastPointerPos = m_pointerPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_transform, eventData.position, eventData.pressEventCamera, out m_pointerPos);
            if (m_pointerPos.x >= m_transform.rect.x && m_pointerPos.x <= m_transform.rect.xMax &&
               m_pointerPos.y >= m_transform.rect.y && m_pointerPos.y <= m_transform.rect.yMax)
            {
                UpdateAxisValues(m_pointerPos - lastPointerPos);
            }
            else
            {
                ResetAxisValues();
                m_isPointerDown = false;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_isPointerDown = false;
        ResetAxisValues();
    }

    private void UpdateAxisValues(Vector2 delta)
    {
        if (m_axesToUse == AxisOption.Both || m_axesToUse == AxisOption.OnlyHorizontal)
            m_horizontal = delta.x * m_sensitivity.x;
        if (m_axesToUse == AxisOption.Both || m_axesToUse == AxisOption.OnlyVertical)
            m_vertical = delta.y * m_sensitivity.y;
    }

    private void ResetAxisValues( )
    {
        m_horizontal = 0.0f;
        m_vertical = 0.0f;
    }

    private void SetHorizontalAxis(float value)
    {
        InputBinding binding = m_horizontalAxisBinding.Get();
        binding.SetRemoteAxisValue(value);
    }

    private void SetVerticalAxis(float value)
    {
        var binding = m_verticalAxisBinding.Get();
        binding.SetRemoteAxisValue(value);
    }
}