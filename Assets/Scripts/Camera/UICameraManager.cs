using System.Linq;
using UnityEngine;

public class UICameraManager : MonoBehaviour
{
    private Camera _mainCamera;

    private GameObject _login;
    private GameObject _title;
    private GameObject _server;
    private GameObject _settings;
    private GameObject _register;


    private CurrentPanel _state;

    public CurrentPanel OnPanel => _state;

    private void Awake( )
    {
        _mainCamera = GameObject.FindGameObjectsWithTag("MainCamera").First().GetComponent<Camera>();
        _register = GameObject.Find("RegistrationScreen");
        _login = GameObject.Find("LogInScreen");
        _title = GameObject.Find("TitleScreen");
        _settings = GameObject.Find("SettingsScreen");
        _server = GameObject.Find("ServerScreen");
        _state = CurrentPanel.Login;
    }

    public void ToLoginPanel( )
    {
        _mainCamera.transform.LookAt(_login.transform);
        _state = CurrentPanel.Login;
    }
    public void ToRegistrationPanel()
    {
        _mainCamera.transform.LookAt(_register.transform);
        _state = CurrentPanel.Register;
    }

    public void ToMainMenu( )
    {
        _mainCamera.transform.LookAt(_title.transform);
        _state = CurrentPanel.Main;
    }

    public void ToServer()
    {
        _mainCamera.transform.LookAt(_server.transform);
        _state = CurrentPanel.Server;
    }

    public void ToSettingsPanel( )
    {
        _mainCamera.transform.LookAt(_settings.transform);
        _state = CurrentPanel.Settings;
    }

    public void ToBack( )
    {
        if (_state == CurrentPanel.Settings)
        {
            _mainCamera.transform.LookAt(_title.transform);
            _state = CurrentPanel.Main;
        } else if (OnPanel == CurrentPanel.Server)
        {
            _mainCamera.transform.LookAt(_title.transform);
            _state = CurrentPanel.Main;
        }
    }
}

public enum CurrentPanel
{
    Main,
    Login,
    Server,
    Register,
    Settings
}
