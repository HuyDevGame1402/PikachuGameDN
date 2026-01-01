using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    None,        
    Playing,     
    Paused,     
    GameOver,    
    Victory      
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int currentLever = 1;
    [SerializeField] private int currentScoreInLevel = 0;
    [SerializeField] private int scoreGame = 0;

    [SerializeField] private GameState gameState;

    [SerializeField] private Board board;

    [SerializeField] private int comboCount = 0;

    public static event Action ONSTARTGAME;
    public static event Action COMBO;
    public static event Action<int> ONCHANGSCORE;

    [SerializeField] private Transform completeUI;
    [SerializeField] private LevelText levelText;

    public bool isNextLevel;

    protected override void Awake()
    {
        base.Awake();
        board = GameObject.Find("Board").GetComponent<Board>();
        if (GameObject.Find("GameData") == null) return;
        GameData gameData = GameObject.Find("GameData").GetComponent<GameData>();

        PikachuGameLogic.WINGAME += WinGameCompleteUI;
        LevelTimeManager.OnTimeOut += LossGameCompleteUI;
        if (gameData != null)
        {
            currentLever = gameData.GetLevelChoose();
            if(levelText != null) levelText.SetLevelText(currentLever);
        }
        gameState = GameState.Playing;
    }

    private void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        if (board != null)
        {
            board.GenerateBoard(LeverManager.Instance.GetLever(currentLever - 1));
        }
        currentScoreInLevel = LeverManager.Instance.GetLever(currentLever - 1).score;
        ONSTARTGAME?.Invoke();
    }
    public int GetCurrentLever()
    {
        return currentLever;
    }
    public void SetCurrentLevel(int level)
    {
        currentLever = level;
    }

    public void SetComboCount()
    {
        comboCount++;
        if(comboCount % 5 == 0)
        {
            COMBO?.Invoke();
        }
    }
    public int GetComboCount()
    {
        return comboCount;
    }
    public void AddScoreGame()
    {
        scoreGame += comboCount * currentScoreInLevel;
        ONCHANGSCORE?.Invoke(scoreGame);
    }
    private void ResetScoreGame()
    {
        scoreGame = 0;
        ONCHANGSCORE?.Invoke(scoreGame);
    }
    public int GetScoreGame()
    {
        return scoreGame;
    }
    public void ResetCombo()
    {
        comboCount = 0;
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }
    public GameState GetGameState()
    {
        return gameState;
    }
    public void WinGameCompleteUI()
    {
        completeUI.GetComponent<CompleteUI>().WinGameUI();
    }
    public void LossGameCompleteUI()
    {
        if (board.IsBoardEmpty())
        {
            WinGameCompleteUI();
        }
        else
        {
            completeUI.GetComponent<CompleteUI>().LossGameUI();
        }
    }

    public void LoadGameLogic()
    {
        if(isNextLevel)
        {
            currentLever++;
        }
        if (levelText != null) levelText.SetLevelText(currentLever);
        InitGame();
        ResetScoreGame();
        gameState = GameState.Playing;
        comboCount = 0;
    }

}
