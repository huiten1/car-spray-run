using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] GameObject LevelCompleteScreen;
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject HUD;
    private GameObject currentScreen;

    private void Start()
    {
        LevelCompleteScreen.SetActive(false);
        StartScreen.SetActive(false);
        HUD.SetActive(false);
        currentScreen = StartScreen;
        currentScreen.SetActive(true);
    }
    public void ShowHud() => SetScreen(HUD);
    public void ShowStartScreen() => SetScreen(StartScreen);
    public void ShowLevelCompleteScreen() => SetScreen(LevelCompleteScreen);

    private void SetScreen(GameObject screen)
    {
        currentScreen.SetActive(false);
        currentScreen = screen;
        currentScreen.SetActive(true);
    }
    // public void SetScreen()
    // Start is called before the first frame update
}
