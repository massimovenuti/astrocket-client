using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonActions : MonoBehaviour
{
    public GameObject loginForm, signupForm;
    public Button loginButton, signupButton;

    private UICameraManager _cam;

    private void Awake( )
    {
        _cam = GameObject.Find("Camera").GetComponent<UICameraManager>();
    }

    public void OnClickLogin( )
    {
        TMP_InputField mdp, user;
        user = GameObject.Find("UsernameField").GetComponent<TMP_InputField>();
        mdp = GameObject.Find("PasswordField").GetComponent<TMP_InputField>();

        if (user == null || mdp == null)
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
        TMP_InputField mdp, user, email, mdpConf;
        user = GameObject.Find("UsernameField").GetComponent<TMP_InputField>();
        email = GameObject.Find("EmailField").GetComponent<TMP_InputField>();
        mdp = GameObject.Find("PasswordField").GetComponent<TMP_InputField>();
        mdpConf = GameObject.Find("PasswordConfirmField").GetComponent<TMP_InputField>();

        if (user == null || email == null || mdp == null || mdpConf == null)
            Debug.LogError("Couldn't find password or username field");
        else
        {
            if (_cam.OnPanel == CurrentPanel.Login)
            {
                // TODO : Register API call here
                if (true)
                    _cam.ToMainMenu();
            }
            else
                _cam.ToLoginPanel();
        }
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

    public void OnClickToggleLogIn( )
    {
        loginForm.SetActive(true);
        signupForm.SetActive(false);
        loginButton.interactable = false;
        signupButton.interactable = true;
    }

    public void OnClickToggleSignUp( )
    {
        loginForm.SetActive(false);
        signupForm.SetActive(true);
        loginButton.interactable = true;
        signupButton.interactable = false;
    }
}
