using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsScreenManager : ScreenManager
{
    private GameObject _panelAudio;
    private GameObject _panelGraphics;
    private GameObject _panelControls;
    private Button _panelAudioButton;
    private Button _panelControlsButton;
    private Button _panelGraphicsButton;
    private Button _saveButton;

    private TMP_Text _musicValue;
    private TMP_Text _masterValue;
    private TMP_Text _effectsValue;
    private Slider _musicVolumeSlider;
    private Slider _masterVolumeSlider;
    private Slider _effectsVolumeSlider;

    void Start( )
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Landscape;
#endif
        base.Start();

        _saveButton = transform.Find("Footer/SaveButton").GetComponent<Button>();

        _panelGraphics = GameObject.Find("GraphicsSettings").gameObject;
        _panelControls = GameObject.Find("ControlsSettings").gameObject;
        _panelAudio = GameObject.Find("AudioSettings").gameObject;

        _panelGraphicsButton = GameObject.Find("GraphicsTabToggler").GetComponent<Button>();
        _panelControlsButton = GameObject.Find("ControlsTabToggler").GetComponent<Button>();
        _panelAudioButton = GameObject.Find("AudioTabToggler").GetComponent<Button>();

        _panelGraphicsButton.onClick.AddListener(( ) => { setPanelTo(Panel.Graphics); });
        _panelControlsButton.onClick.AddListener(( ) => { setPanelTo(Panel.Controls); });
        _panelAudioButton.onClick.AddListener(( ) => { setPanelTo(Panel.Audio); });

        _musicValue = GameObject.Find("MusicVolumeValue").GetComponent<TMP_Text>();
        _masterValue = GameObject.Find("MasterVolumeValue").GetComponent<TMP_Text>();
        _effectsValue = GameObject.Find("EffectsVolumeValue").GetComponent<TMP_Text>();

        _musicVolumeSlider = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
        _masterVolumeSlider = GameObject.Find("MasterVolumeSlider").GetComponent<Slider>();
        _effectsVolumeSlider = GameObject.Find("EffectsVolumeSlider").GetComponent<Slider>();

        _musicVolumeSlider.onValueChanged.AddListener(updateMusicValue);
        _masterVolumeSlider.onValueChanged.AddListener(updateMasterValue);
        _effectsVolumeSlider.onValueChanged.AddListener(updateEffectsValue);

        _saveButton.onClick.AddListener(saveSettings);

        setPanelTo(Panel.Graphics);
        updateMasterValue(_masterVolumeSlider.value);
        updateMusicValue(_masterVolumeSlider.value);
        updateEffectsValue(_masterVolumeSlider.value);

    }

    private void saveSettings( )
    {
        /*
         * TODO
         */
    }

    private void updateMusicValue(float value)
    {
        _musicValue.text = value.ToString();
    }
    private void updateMasterValue(float value)
    {
        _masterValue.text = value.ToString();
    }

    private void updateEffectsValue(float value)
    {
        _effectsValue.text = value.ToString();
    }

    void Update( )
    {

    }

    public void SetMasterVolume(float val)
    {
        _masterVolumeSlider.value = val;
    }

    public void SetMusicVolume(float val)
    {
        _musicVolumeSlider.value = val;
    }

    private void setPanelTo(Panel panel)
    {
        if (panel == Panel.Graphics)
        {
            _panelGraphicsButton.interactable = false;
            _panelControlsButton.interactable = true;
            _panelAudioButton.interactable = true;
            _panelGraphics.SetActive(true);
            _panelControls.SetActive(false);
            _panelAudio.SetActive(false);
        }
        else if (panel == Panel.Controls)
        {
            _panelGraphicsButton.interactable = true;
            _panelControlsButton.interactable = false;
            _panelAudioButton.interactable = true;
            _panelGraphics.SetActive(false);
            _panelControls.SetActive(true);
            _panelAudio.SetActive(false);
        }
        else if (panel == Panel.Audio)
        {
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
