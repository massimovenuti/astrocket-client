using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject nextPage;
    public GameObject previousPage;

    protected void goToPreviousPage()
    {
        if (previousPage == null) { return; }
        gameObject.SetActive(false);
        previousPage.SetActive(true);
    }

    protected void goToNextPage()
    {
        if (nextPage == null) { return; }
        gameObject.SetActive(false);
        nextPage.SetActive(true);
    }
}
