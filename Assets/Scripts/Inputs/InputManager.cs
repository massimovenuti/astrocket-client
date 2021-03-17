using UnityEngine;
using System.Collections.Generic;
using System.Linq;

class InputManager : MonoBehaviour
{

    private bool _isUsingController;
    private InputManager _instance = null;
    private Dictionary<string, KeyCode> _keys;
    private Joystick _joystick;
    private UIButtonPressHandler _shoot;
    private UIButtonPressHandler _boost;

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
#if UNITY_ANDROID
            GameObject.Find("Canvas").transform.Find("MobileControls").gameObject.SetActive(true);
            _joystick = FindObjectOfType<Joystick>();
            _shoot = FindObjectsOfType<UIButtonPressHandler>().Where(o => o.name.Equals("Shoot")).First();
            _boost = FindObjectsOfType<UIButtonPressHandler>().Where(o => o.name.Equals("Boost")).First();
#endif
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

    public string SaveInputs()
    {
        string json = $"\"controls\" : {{";
        foreach (var kp in _keys)
            json += $"\"{kp.Key}\" : \"{kp.Value}\",";
        json += $"\" }}";
        return json;
    }
}
