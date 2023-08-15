using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : Singleton<CanvasManager>
{
    [SerializeField] GameObject LevelCompleteScreen;
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject PaintSectionScreen;
    [SerializeField] ProgressBar paintProgressBar;
    [SerializeField] TMPro.TMP_Text levelText;

    private GameObject currentScreen;

    private IEnumerator Start()
    {
        LevelCompleteScreen.SetActive(false);
        StartScreen.SetActive(false);
        HUD.SetActive(false);
        PaintSectionScreen.SetActive(false);

        currentScreen = StartScreen;
        currentScreen.SetActive(true);
        yield return null;
        levelText.SetText($"Level {GameManager.Instance.gameData.currentLevel + 1}");
    }
    public void ShowHud() => SetScreen(HUD);
    public void ShowStartScreen() => SetScreen(StartScreen);
    public void ShowLevelCompleteScreen() => SetScreen(LevelCompleteScreen);
    public void ShowPaintSectionUI() => SetScreen(PaintSectionScreen);
    private void SetScreen(GameObject screen)
    {
        currentScreen.SetActive(false);
        currentScreen = screen;
        currentScreen.SetActive(true);
    }
    public void SetPaintProgressTrackingObject(GameObject objectToTrack)
    {
        paintProgressBar.SetTrackingObject(objectToTrack);
    }
    // public void SetScreen()
    // Start is called before the first frame update
}
