using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menuCanvas;

    private Button resumeButton;
    private Button settingsButton; // TODO
    private Button disconnectButton;

    private void Awake( )
    {
        menuCanvas.SetActive(false);

        resumeButton = menuCanvas.FindObjectByName("ResumeButton").GetComponent<Button>();
        resumeButton.onClick.AddListener(SetMenuState);

#if UNITY_ANDROID
        resumeButton.onClick.AddListener(() => {
            menuCanvas.transform.parent.Find("Canvas/MobileControls/Menu").gameObject.SetActive(true);
        });
#endif


        disconnectButton = menuCanvas.FindObjectByName("DisconnectButton").GetComponent<Button>();
        disconnectButton.onClick.AddListener(GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().StopClient);
    }

    private void Update()
    {
        if(InputManager.InputManagerInst.ShowMenu())
        {
            SetMenuState();
        }
    }

    private void SetMenuState()
    {
        menuCanvas.SetActive(!menuCanvas.activeSelf);
    }
}
