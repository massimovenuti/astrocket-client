using UnityEngine;
using System.Collections.Generic;
using System.Linq;

class InputManager
{
    public static readonly Dictionary<string, KeyCode> DefaultKeys =
        new Dictionary<string, KeyCode>()
        {
            ["Score"] = KeyCode.Tab,
            ["Boost"] = KeyCode.Space,
            ["Shoot"] = KeyCode.Mouse0,
        };

    private static InputManager _instance = null;

    private bool _isUsingController;
    private static Dictionary<string, KeyCode> _keys = null;

    private Joystick _joystick;
    private UIButtonPressHandler _menu;
    private UIButtonPressHandler _shoot;
    private UIButtonPressHandler _boost;

    public static InputManager InputManagerInst
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Init InputManager");
                _instance = new InputManager();
                _instance._isUsingController = false;
                if (_keys == null)
                    _keys = DefaultKeys;
            }
            return _instance;
        }
    }

    public void RegisterMobileUser(GameObject canvas)
    {
        _joystick = canvas.GetComponentInChildren<Joystick>(true);
        _boost = canvas.transform.Find("Boost").GetComponent<UIButtonPressHandler>();
        _shoot = canvas.transform.Find("Shoot").GetComponent<UIButtonPressHandler>();
        _menu = canvas.transform.Find("Menu").GetComponent<UIButtonPressHandler>();
        Screen.orientation = ScreenOrientation.LandscapeRight;

    }

    public bool SetKeyForAxis(string axis, KeyCode key)
    {
        Debug.Assert(_keys.ContainsKey(axis));
        if (key != KeyCode.Escape && !(_keys.ContainsValue(key) && _keys[axis] != key))
        {
            Debug.Log($"inp set: {axis}, {key}");
            _keys[axis] = key;
        }
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
        if(true)
            return _shoot.IsToggled;
        else
            return _shoot.IsPressed;
#else
        return Input.GetKey(_keys["Shoot"]);
#endif
    }

    public bool ShowScoreboard( )
    {
#if UNITY_ANDROID
        return false;
#else
        return Input.GetKey(_keys["Score"]);
#endif
    }

    public bool ShowMenu( )
    {
#if UNITY_ANDROID
        if (_menu.IsClicked)
        {
            _menu.gameObject.SetActive(false);
            return true;
        } else
            return false;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }

    public Key[] SaveInputs( )
    {
        Key[] arr = new Key[_keys.Count];
        int i = 0;
        foreach(var v in _keys)
        {
            arr[i] = new Key();
            arr[i].keyname = v.Key;
            arr[i].key = v.Value;
            Debug.Log($"Saving : {v.Key} {v.Value}");
            i++;
        }
        return arr;   
    }
}
