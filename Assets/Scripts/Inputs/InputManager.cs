using UnityEngine;
using System.Collections.Generic;

class InputManager : MonoBehaviour
{

    private bool _isUsingController;
    private InputManager _instance = null;
    private Dictionary<string, KeyCode> _keys;

    public InputManager InputManagerInst
    {
        get => _instance;
    }

    private void OnEnable( )
    {
        if (_instance == null)
        {
            _instance = this;
            _isUsingController = false;
            _keys = new Dictionary<string, KeyCode>(2)
            {
                ["Boost"] = KeyCode.Space,
                ["Shoot"] = KeyCode.Mouse0
            };
        }
        else
        {
            Destroy(this);
            Debug.LogError($"Only one InputManager may be present in the scene at a given time");
        }
        Debug.Log($"OnEnable ran {_instance == null}");
    }

    public void SetKeyForAxis(string axis, KeyCode key)
    {
        Debug.Assert(_keys.ContainsKey(axis));
        if (key != KeyCode.Escape)
            _keys[axis] = key;
    }

    public Vector3 PointingAt( )
    {
#if UNITY_ADROID
        return Input.GetTouch(0).position;
#else
        return Input.mousePosition;
#endif
    }

    public bool IsBoosting( )
    {
        if (_isUsingController)
            return true;
        else
            return Input.GetKey(_keys["Boost"]);
    }

    public bool IsShooting( )
    {
        if (_isUsingController)
            return true;
        else
            return Input.GetKey(_keys["Shoot"]);
    }
}
