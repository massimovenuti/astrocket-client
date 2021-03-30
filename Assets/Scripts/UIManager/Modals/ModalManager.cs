using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ModalManager : MonoBehaviour
{
    public string title;
    public string content;

    void Start()
    {
        Button closeButton;
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(hide);
        setTitleValue(title);
        setContentValue(content);
    }

    protected void setTitleValue(string newTitle)
    {
        changeTextElementValue("ModalTitle", newTitle);
    }

    protected void setContentValue(string newContent)
    {
        changeTextElementValue("ModalContent", newContent);
    }

    private void changeTextElementValue(string elementName, string newValue)
    {
        TMP_Text textElement;

        try
        {
            textElement = transform.Find(elementName).GetComponent<TMP_Text>();
            textElement.text = newValue;
        } catch(Exception e)
        {
            // ¯\_(ツ)_/¯
        }
    }

    protected void hide()
    {
        gameObject.SetActive(false);
    }
}
