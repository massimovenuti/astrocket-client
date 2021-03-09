using UnityEngine;
using TMPro;



public class ButtonActions : MonoBehaviour
{
    private UICameraManager _cam;


    private void Awake( )
    {
        _cam = GameObject.Find("Camera").GetComponent<UICameraManager>();
    }

    public void OnClickLogin( )
    {
        TMP_InputField mdp, user;
        user = GameObject.Find("Username").GetComponent<TMP_InputField>();
        mdp = GameObject.Find("Password").GetComponent<TMP_InputField>();

        if(user == null || mdp == null)
            Debug.LogError("Couldn't find password or username field");
        else
        {
            if (_cam.OnPanel == CurrentPanel.Login)
            {
                // TODO : Login API call here
                if (true)
                    _cam.ToMainMenu();
            }
            else
                _cam.ToLoginPanel();


        }        
    }

    public void OnClickRegister( )
    {
        if(_cam.OnPanel == CurrentPanel.Register)
        {
            // TODO : API call here
        }
        _cam.ToRegistrationPanel();
    }

    public void OnClickLogOut()
    {
        // TODO : API Call here
        _cam.ToLoginPanel();
    }

    public void OnClickServerList( )
    {
        // TODO : API request server list
        _cam.ToServer();
    }

    public void OnClickSettings( )
    {
        _cam.ToSettingsPanel();
    }

    public void OnClickBack( )
    {
        _cam.ToBack();
    }

    public void OnClickSaveSettings( )
    {

    }

    public void OnClickExit( )
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
