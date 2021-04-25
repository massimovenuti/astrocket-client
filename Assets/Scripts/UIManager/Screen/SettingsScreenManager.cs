using UnityEngine;
using UnityEngine.UI;

public class SettingsScreenManager : ScreenManager
{
    private GameObject _panelGraphics;
    private GameObject _panelControls;
    private GameObject _panelAudio;
    private Button _panelGraphicsButton;
    private Button _panelControlsButton;
    private Button _panelAudioButton;

    void Start( )
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Landscape;
#endif
        base.Start();

        _panelGraphics = GameObject.Find("GraphicsSettings").gameObject;
        _panelControls = GameObject.Find("ControlsSettings").gameObject;
        _panelAudio = GameObject.Find("AudioSettings").gameObject;
        
        _panelGraphicsButton = GameObject.Find("GraphicsTabToggler").GetComponent<Button>();
        _panelControlsButton = GameObject.Find("ControlsTabToggler").GetComponent<Button>();
        _panelAudioButton = GameObject.Find("AudioTabToggler").GetComponent<Button>();

        _panelGraphicsButton.onClick.AddListener(( ) => { setPanelTo(Panel.Graphics); });
        _panelControlsButton.onClick.AddListener(( ) => { setPanelTo(Panel.Controls); });
        _panelAudioButton.onClick.AddListener(( ) => { setPanelTo(Panel.Audio); });

        setPanelTo(Panel.Graphics);
    }

    void Update( )
    {

    }

    private void setPanelTo(Panel panel)
    {
        if (panel == Panel.Graphics) {
            _panelGraphicsButton.interactable = false;
            _panelControlsButton.interactable = true;
            _panelAudioButton.interactable = true;
            _panelGraphics.SetActive(true);
            _panelControls.SetActive(false);
            _panelAudio.SetActive(false);
        } else if (panel == Panel.Controls) {
            _panelGraphicsButton.interactable = true;
            _panelControlsButton.interactable = false;
            _panelAudioButton.interactable = true;
            _panelGraphics.SetActive(false);
            _panelControls.SetActive(true);
            _panelAudio.SetActive(false);
        } else if (panel == Panel.Audio) {
            _panelGraphicsButton.interactable = true;
            _panelControlsButton.interactable = true;
            _panelAudioButton.interactable = false;
            _panelGraphics.SetActive(false);
            _panelControls.SetActive(false);
            _panelAudio.SetActive(true);
        }
    }

    enum Panel
    {
        Graphics,
        Controls,
        Audio
    }
}
