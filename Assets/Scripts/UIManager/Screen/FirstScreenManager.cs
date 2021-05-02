using UnityEngine;

public class FirstScreenManager : ScreenManager
{
    public override void Start()
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
