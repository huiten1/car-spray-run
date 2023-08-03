using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool isPlaying { get; private set; }
    public GamePhase currentPhase { get; private set; }
    private void SetPhase(GamePhase phase) { currentPhase = phase; onPhaseChange?.Invoke(currentPhase); }
    private void Start()
    {
        Player.Instance.OnEnterPaintSection += HandleEnterPaintSection;
    }

    private void HandleEnterPaintSection(PaintSection section)
    {
        SetPhase(GamePhase.Paint);
        section.isPainting = true;
        section.onTankEmpty += () =>
        {
            section.isPainting = false;
            SetPhase(GamePhase.Runner);

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

    }
}
