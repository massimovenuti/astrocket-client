using UnityEngine;
using System.Collections.Generic;
using System.Linq;

class InputManager : MonoBehaviour
{

    private static InputManager _instance = null;
    
    private bool _isUsingController;
    private Dictionary<string, KeyCode> _keys;
    private Joystick _joystick;
    private UIButtonPressHandler _shoot;
    private UIButtonPressHandler _boost;

    public static InputManager InputManagerInst
    {
        get => _instance;
    }

    private void Awake( )
    {
        if (_instance == null)
        {
            _instance = this;
            _isUsingController = false;
            _keys = new Dictionary<string, KeyCode>(2)
            {
                ["Boost"] = KeyCode.Space,
                ["Shoot"] = KeyCode.Mouse0,
                ["Score"] = KeyCode.Tab
            };
        }
        else
        {
            Destroy(this);
            Debug.LogError($"Only one InputManager may be present in the scene at a given time");
        }
        Debug.Log($"OnEnable ran {_instance == null}");
    }

    public void RegisterMobileUser(GameObject canvas)
    {
        Debug.Log($"{canvas is null} {canvas.activeInHierarchy}");
        _joystick = canvas.GetComponentInChildren<Joystick>();
        Debug.Log($"Joystick {_joystick is null}");
        _boost = canvas.transform.Find("Boost").GetComponent<UIButtonPressHandler>();
        Debug.Log($"boost {_boost is null}");
        _shoot = canvas.transform.Find("Shoot").GetComponent<UIButtonPressHandler>();
        Debug.Log($"shoot {_boost is null}");

    }

    public bool SetKeyForAxis(string axis, KeyCode key)
    {
        Debug.Assert(_keys.ContainsKey(axis));
        if (key != KeyCode.Escape)
            _keys[axis] = key;
        else
            return false;
        return true;
    }

    public Vector3 PointingAt( )
    {
#if UNITY_ANDROID
        Vector3 v = Vector3.zero;
        v.y = 0f;
        if (_joystick.Horizontal < .1f && _joystick.Horizontal > -.1f)
            v.x = 0f;
        else
            v.x = _joystick.Horizontal;

        if (_joystick.Vertical < .1f && _joystick.Vertical > -.1f)
            v.z = 0f;
        else
            v.z = _joystick.Horizontal;
        return new Vector3(_joystick.Direction.x, 0f, _joystick.Direction.y);
#else
        return Input.mousePosition;
#endif
    }

    public bool IsBoosting( )
    {
#if UNITY_ANDROID
        return _boost.IsPressed;
#else
        if (_isUsingController)
            return true;
        else
            return Input.GetKey(_keys["Boost"]);
#endif
    }

    public bool IsShooting( )
    {
#if UNITY_ANDROID
        return _shoot.IsPressed;
#else
        return Input.GetKey(_keys["Shoot"]);
#endif
    }

    public bool ShowScoreboard()
    {
#if UNITY_ANDROID
        return false;
#else
        return Input.GetKey(_keys["Score"]);
#endif
    }

    public Keys SaveInputs( )
    {
        InputKey[] ks = {
            new InputKey("Shoot", _keys["Shoot"]),
            new InputKey("Boost", _keys["Boost"])
        };

        Keys k = new Keys(ks);

        return k;
    }
}
