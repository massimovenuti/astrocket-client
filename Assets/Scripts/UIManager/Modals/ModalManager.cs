using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ModalManager : MonoBehaviour
{
    public Button[] modalButtons;

    void Start()
    {
        Button closeButton;
        closeButton = transform.Find("ModalBody/Footer/CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(Hide);
    }

    public void SetTitleValue(string newTitle)
    {
        changeTextElementValue("ModalBody/ModalTitle", newTitle);
    }

    public void SetContentValue(string newContent)
    {
        changeTextElementValue("ModalBody/ModalContent", newContent);
    }

    private void changeTextElementValue(string elementName, string newValue)
    {
        TMP_Text textElement;

        try
        {
            textElement = transform.Find(elementName).GetComponent<TMP_Text>();
            textElement.text = newValue;
        }
        catch (Exception) { }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
