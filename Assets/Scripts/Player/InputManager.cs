using UnityEngine;
using System.Collections.Generic;

class InputManager : MonoBehaviour
{
    private InputManager _instance = null;
    private Dictionary<string, KeyCode> _keys;
    private bool _isUsingController = false;

    public InputManager InputManagerInst
    {
        get => _instance;
    }

    private void Awake( )
    {
        if (_instance == null)
        {
            _instance = this;
            _keys["Boost"] = KeyCode.None;
            _keys["Shoot"] = KeyCode.None;
        }
        else
            Destroy(this);
    }

    public void SetKeyForAxis(string axis, KeyCode key)
    {
        Debug.Assert(_keys.ContainsKey(axis));
        if (key != KeyCode.Escape)
            _keys[axis] = key;
    }

    public Vector2 LookingAt()
    {

        return Vector2.zero;
    }

    public bool IsBoosting()
    {
        if (_isUsingController)
            return true;
        else
            return true;
    }

    public bool IsShooting( )
    {
        if (_isUsingController)
            return true;
        else
            return true;
    }
}
