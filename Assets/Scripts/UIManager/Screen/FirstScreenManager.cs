using API;
using API.Auth;
using UnityEngine;

public class FirstScreenManager : ScreenManager
{
    private AuthAPICall _auth = new AuthAPICall();
    public override void Start()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
#endif
        base.Start();
        if (SharedInfo.userToken != null)
        {
            UserRole u = _auth.PostCheckUserToken(SharedInfo.userToken);
            if (_auth.ErrorMessage.IsOk)
            {
                SharedInfo.HasValidatedToken = true;
                goToNextPage();
            }
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
            goToNextPage();
    }
}
