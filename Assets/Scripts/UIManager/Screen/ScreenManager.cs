﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public GameObject nextPage;
    public GameObject previousPage;

    private GameObject m_popup;

    public virtual void Start( ) // on start : footer handling
    {
        Button goBackButton, exitButton;

        try
        {
            goBackButton = GameObject.Find("GoBackButton").GetComponent<Button>();
            if (goBackButton != null)
            {
                goBackButton.onClick.AddListener(goToPreviousPage);
            }
        }
        catch { }

        try
        {
            exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(exitGame);
            }
        }
        catch { }

        
    }

    protected void checkBackKey()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            goToPreviousPage();
        }
#endif
    }

    protected void goToPage(GameObject page)
    {
        if (page == null) { return; }
        gameObject.SetActive(false);
        page.SetActive(true);
    }

    protected void goToPreviousPage()
    {
        goToPage(previousPage);
    }

    protected void goToNextPage()
    {
        goToPage(nextPage);
    }

    protected void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
