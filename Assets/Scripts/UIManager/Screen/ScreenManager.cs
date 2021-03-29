using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public GameObject nextPage;
    public GameObject previousPage;

    public void Start( ) // on start : footer handling
    {
        Button goBackButton, exitButton;

        try
        {
            goBackButton = GameObject.Find("GoBackButton").GetComponent<Button>();
            if (goBackButton != null)
            {
                goBackButton.onClick.AddListener(goToPreviousPage);
            }
        } catch (Exception e)
        {
            Debug.LogError("No Back button found in the footer"); // can happen if we don't want to go back
        }

        try
        {
            exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(exitGame);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("No Exit button found in the footer"); // can happen if we don't want to exit
        }
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
