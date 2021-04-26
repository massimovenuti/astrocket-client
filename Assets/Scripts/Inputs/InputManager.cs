using UnityEngine;
using System.Collections.Generic;
using System.Linq;

class InputManager : MonoBehaviour
{

    private static InputManager _instance = null;
    
    private bool _isUsingController;
    private Dictionary<string, KeyCode> _keys;

    private Joystick _joystick;
    private UIButtonPressHandler _menu;
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
        _joystick = canvas.GetComponentInChildren<Joystick>();
        _boost = canvas.transform.Find("Boost").GetComponent<UIButtonPressHandler>();
        _shoot = canvas.transform.Find("Shoot").GetComponent<UIButtonPressHandler>();
        _menu = canvas.transform.Find("Menu").GetComponent<UIButtonPressHandler>();

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

    public bool ShowMenu()
    {
#if UNITY_ANDROID
        if(_menu.IsClicked)
        {
            _menu.gameObject.SetActive(false);
            return true;
        } else
            return false;
#else
        return Input.GetKeyDown(KeyCode.Escape);
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
