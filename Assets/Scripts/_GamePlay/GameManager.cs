using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private void SetPhase(GamePhase phase) { currentPhase = phase; onPhaseChange?.Invoke(currentPhase); }
    private void Start()
    {
        PaintSection.OnPlayerEnterPaintSection += HandleEnterPaintSection;
        gameData = SaveManager.Load<GameData>();
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
        section.onTankEmpty += () =>
        {
            section.isPainting = false;
            SetPhase(GamePhase.Runner);
            CanvasManager.Instance.ShowHud();
        };
        section.car.onPainted += async () =>
        {
            for (float i = 0; i < 0.6f; i += Time.deltaTime)
            {
                await Task.Yield();
            }
            section.isPainting = false;
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
        isPlaying = false;
    }
    public void LevelComplete()
    {
        isPlaying = false;
        onGameStop?.Invoke();
        CanvasManager.Instance.ShowLevelCompleteScreen();
    }
    public void Restart()
    {
        Reload();
    }
    internal void Reload()
    {
        SceneManager.LoadScene("Main");
    }
}
