using UnityEngine;
using UnityEngine.UI;

public class StartScreenManager : ScreenManager
{
    public GameObject playPage;
    public GameObject statsPage;
    public GameObject settingsPage;


    new void Start()
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Portrait;
#endif
        base.Start();

        Button playButton, statsButton, settingsButton;

        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        statsButton = GameObject.Find("StatsButton").GetComponent<Button>();
        settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();

        playButton.onClick.AddListener(( ) => goToPage(playPage));
        statsButton.onClick.AddListener(( ) => goToPage(statsPage));
        settingsButton.onClick.AddListener(( ) => goToPage(settingsPage));
    }
}
