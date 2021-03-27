using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthScreenManager : ScreenManager
{
    private GameObject _loginForm;
    private GameObject _signupForm;
    private Button _loginToggleButton;
    private Button _signupToggleButton;

    void Start()
    {
        Button loginButton, signupButton;

#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
#endif

        loginButton = GameObject.Find("LoginSubmitButton").GetComponent<Button>();
        signupButton = GameObject.Find("SignUpSubmitButton").GetComponent<Button>();

        loginButton.onClick.AddListener(OnClickLogin);
        signupButton.onClick.AddListener(OnClickSignUp);

        _loginForm = GameObject.Find("LoginForm");
        _signupForm = GameObject.Find("SignUpForm");
        _loginToggleButton = GameObject.Find("LoginToggleButton").GetComponent<Button>();
        _signupToggleButton = GameObject.Find("SignUpToggleButton").GetComponent<Button>();

        _loginToggleButton.onClick.AddListener(OnClickToggleLogIn);
        _signupToggleButton.onClick.AddListener(OnClickToggleSignUp);
        
        _signupForm.SetActive(false);
    }

    void OnClickToggleLogIn()
    {
        _loginForm.SetActive(true);
        _signupForm.SetActive(false);
        _loginToggleButton.interactable = false;
        _signupToggleButton.interactable = true;
    }

    void OnClickToggleSignUp()
    {
        _loginForm.SetActive(false);
        _signupForm.SetActive(true);
        _loginToggleButton.interactable = true;
        _signupToggleButton.interactable = false;
    }

    void OnClickLogin()
    {
        TMP_InputField mdp, user;
        user = GameObject.Find("UsernameField").GetComponent<TMP_InputField>();
        mdp = GameObject.Find("PasswordField").GetComponent<TMP_InputField>();

        if (user != null && mdp != null)
        {
            if (true) // TODO : Login API call -> if login succeeded
            {
                goToNextPage();
            }
            // else { showError(...); }
        }
    }

    void OnClickSignUp()
    {
        TMP_InputField mdp, user, email, mdpConf;
        user = GameObject.Find("UsernameField").GetComponent<TMP_InputField>();
        email = GameObject.Find("EmailField").GetComponent<TMP_InputField>();
        mdp = GameObject.Find("PasswordField").GetComponent<TMP_InputField>();
        mdpConf = GameObject.Find("PasswordConfirmField").GetComponent<TMP_InputField>();

        if (user != null && email != null && mdp != null && mdpConf != null)
        { 
            if (mdp.text == mdpConf.text)
            {
                if (true) // TODO : Signup API call -> if signup succeeded & auth token received
                {
                    goToNextPage();
                }
                // else { showError(...); } 
            }
            // else { showError(...); }
        }
    }
}
