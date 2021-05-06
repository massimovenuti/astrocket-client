using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsSettingsManager : MonoBehaviour
{
    public string cellName;
    public string controlName;

    private bool _isWaiting;
    private string _keyName;
    private KeyCode _keyCode;
    private bool _isListening;
    private GameObject _ctaEl;
    private TMP_Text _keyNameField;

    void Awake()
    {
        Debug.Log("start");
        TMP_Text cellNameEl = transform.Find("Editor/KeyName").GetComponent<TMP_Text>();
        cellNameEl.text = cellName;

        _isWaiting = false;
        _isListening = false;
        _ctaEl = transform.Find("CallToAction").gameObject;
        _ctaEl.SetActive(false);

        Button listenButton = transform.Find("Editor/KeyChangerButton").GetComponent<Button>();
        listenButton.onClick.AddListener(toggleWaiting);

        _keyNameField = transform.Find("Editor/KeyChangerButton/KeyValue").GetComponent<TMP_Text>();
        SetKeyCode(KeyCode.Space);
    }

    void Update()
    {
        if (_isListening)
        {
            listenToInput();
        }
    }

    private void toggleWaiting()
    {
        _isWaiting = !_isWaiting;
    }

    private void listenToInput( )
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(key))
            {
                SetKeyCode(key);
                SetListening(false);
                break;
            }
        }
    }

    private void updateKeyName()
    {
        _keyName = _keyCode.ToString();
        _keyNameField.text = "<" + _keyName + ">";
    }

    public void SetWaiting(bool isWaiting)
    {
        _isWaiting = isWaiting;
    }

    public void SetListening (bool isListening)
    {
        _isListening = isListening;
        _isWaiting = false;
        _ctaEl.SetActive(isListening);
    }

    public bool GetWaiting ()
    {
        return _isWaiting;
    }

    public KeyCode GetKeyCode()
    {
        return _keyCode;
    }

    public void SetKeyCode(KeyCode keyCode)
    {
        if(InputManager.InputManagerInst.SetKeyForAxis(controlName, keyCode))
        {
            _keyCode = keyCode;
            updateKeyName();
        }
    }
}
