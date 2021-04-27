using UnityEngine;

public class FirstScreenManager : ScreenManager
{
    new void Start()
    {
        SaveManager.Load();
        base.Start();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            goToNextPage();
    }
}
