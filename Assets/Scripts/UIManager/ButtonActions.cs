using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;



public class ButtonActions : MonoBehaviour
{
    private UICameraManager _cam;
    private void Awake( )
    {
        _cam = GameObject.Find("Camera").GetComponent<UICameraManager>();
    }

    public void OnLogin()
    {
        TMP_InputField mdp, user;
        user = GameObject.Find("Username").GetComponent<TMP_InputField>();
        mdp = GameObject.Find("Password").GetComponent<TMP_InputField>();

        if(user == null || mdp == null)
            Debug.LogError("Couldn't find password or username field");
        else
        {
            // Do API call here
            if(true)
            {
                _cam.ToMainMenu();
            }
        }        
    }

    public void OnSignIn()
    {

    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
