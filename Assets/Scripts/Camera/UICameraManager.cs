using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICameraManager : MonoBehaviour
{
    Camera _mainCamera;
    GameObject _loginPanel;
    GameObject _mainMenuPanel;
    GameObject _optionPanel;

    private void Awake( )
    {
        _mainCamera = GameObject.FindGameObjectsWithTag("MainCamera").First().GetComponent<Camera>();
    }

    public void ToLoginPanel( )
    {
        _mainCamera.transform.LookAt(_loginPanel.transform);
    }

    public void ToMainMenu( )
    {
        _mainCamera.transform.rotation = Quaternion.Euler(0, 75, 0);
    }

    public void ToOptionPanel( )
    {
        _mainCamera.transform.LookAt(_optionPanel.transform);
    }
}
