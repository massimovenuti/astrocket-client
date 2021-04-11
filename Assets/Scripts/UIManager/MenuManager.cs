﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MenuManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject menuCanvas;

    private Button resumeButton;
    private Button disconnectButton;

    private void Awake( )
    {
        menuCanvas.SetActive(false);

        resumeButton = menuCanvas.FindObjectByName("ResumeButton").GetComponent<Button>();
        resumeButton.onClick.AddListener(SetMenuState);

        disconnectButton = menuCanvas.FindObjectByName("DisconnectButton").GetComponent<Button>();
        disconnectButton.onClick.AddListener(GameObject.Find("NetworkManager").GetComponent<AsteroidNetworkManager>().StopClient);
    }

    private void Update()
    {
        if(isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SetMenuState();
        }
    }

    private void SetMenuState()
    {
        menuCanvas.SetActive(!menuCanvas.activeSelf);
    }
}