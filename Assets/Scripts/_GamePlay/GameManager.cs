using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameAnalyticsSDK;
// using SupersonicWisdomSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public event System.Action<GamePhase> onPhaseChange;
    public event System.Action onGameStart;
    public event System.Action onGameStop;
    public enum GamePhase
    {
        Runner,
        Paint
    }
    public GameData gameData;
    public bool isPlaying { get; private set; }
    public GamePhase currentPhase { get; private set; }
    public int Gold { get => gameData.playerGold; set => gameData.playerGold = value; }
    private void SetPhase(GamePhase phase)
    {
        if (currentPhase == phase) return;
        currentPhase = phase;
        onPhaseChange?.Invoke(currentPhase);
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        PaintSection.OnPlayerEnterPaintSection += HandleEnterPaintSection;
        gameData = SaveManager.Load<GameData>();
        var levelGenerator = FindObjectOfType<LevelGenerator>();
        gameData.color = levelGenerator.CurrentLevel.paintColor;
        levelGenerator.Generate();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,$"Level_{gameData.currentLevel}");
        // SupersonicWisdom.Api.NotifyLevelStarted(gameData.currentLevel,null);
    }
    private void OnDestroy()
    {
        PaintSection.OnPlayerEnterPaintSection -= HandleEnterPaintSection;
    }

    private void HandleEnterPaintSection(PaintSection section)
    {

        SetPhase(GamePhase.Paint);
        section.isPainting = true;
        CanvasManager.Instance.ShowPaintSectionUI();
        section.OnTankEmpty += () =>
        {
            var currentSection = section;
            currentSection.isPainting = false;
            SetPhase(GamePhase.Runner);
            CanvasManager.Instance.ShowHud();

        };
        section.car.onPainted += async () =>
        {
            var currentSection = section;
            currentSection.isPainting = false;
            for (float t = 0; t < 0.8f; t += Time.deltaTime)
            {
                await Task.Yield();
            }
            SetPhase(GamePhase.Runner);
            CanvasManager.Instance.ShowHud();

        };

    }



    public void GameStart()
    {
        isPlaying = true;
        CanvasManager.Instance.ShowHud();
        onGameStart?.Invoke();
    }
    public void GameOver()
    {
        // SupersonicWisdom.Api.NotifyLevelFailed(gameData.currentLevel,null);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail,$"Level_{gameData.currentLevel}");
        isPlaying = false;
    }
    public void LevelComplete()
    {
        isPlaying = false;
        onGameStop?.Invoke();
        gameData.currentLevel++;
        CanvasManager.Instance.ShowLevelCompleteScreen();
        // SupersonicWisdom.Api.NotifyLevelCompleted(gameData.currentLevel,null);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,$"Level_{gameData.currentLevel}");
        SaveManager.Save(gameData);
    }
    public void Restart()
    {
        Reload();
    }
    public void Reload()
    {
        SceneManager.LoadScene("Main");
    }
    private void OnApplicationQuit()
    {
        SaveManager.Save(gameData);
    }
}
