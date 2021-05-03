using UnityEngine;
using UnityEngine.UI;
using TMPro;
using API.Auth;

public class AuthScreenManager : ScreenManager
{
    private GameObject _loginForm;
    private GameObject _signupForm;
    private Button _loginToggleButton;
    private Button _signupToggleButton;

    private AuthAPICall _auth = new AuthAPICall();

    private UserToken _tok = null;

public override void Start()
    {
        base.Start();

        Button loginButton, signupButton;

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
            _tok = _auth.PostLoginUser(new UserLogin() { Name = user.text, Password = mdp.text});
            Debug.Log($"{_auth.ErrorMessage}");
            if (_auth.ErrorMessage.IsOk)
            {
                _tok.Name = user.text;
                GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().token = _tok.Token;
                SharedInfo.userToken = _tok;
                goToNextPage();
            }
            else 
            {
                _tok = null;
                Debug.Log($"{_auth.ErrorMessage}");
                //showError(...); 
            }
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
                _tok = _auth.PostAddUser(new UserRegister() { Name = user.text, Email = email.text, Password = mdp.text });
                Debug.Log($"{_auth.ErrorMessage}");
                if (_auth.ErrorMessage.IsOk) // TODO : Signup API call -> if signup succeeded & auth token received
                {
                    _tok.Name = user.text;
                    SharedInfo.userToken = _tok;
                    goToNextPage();
                }
                else
                {
                    _tok = null;
                }
                // else { showError(...); } 
            }
            // else { showError(...); }
        }
    }
}
