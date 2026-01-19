using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public enum GameState
{
    None,        
    Playing,     
    Paused,     
    GameOver,    
    Victory      
}

public enum CandyType
{
    None, 
    Vertical,
    Horizontal,
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int currentLever = 1;
    [SerializeField] private int currentScoreInLevel = 0;
    [SerializeField] private int scoreGame = 0;
    [SerializeField] private int currentStar = 0;

    [SerializeField] private GameState gameState;

    [SerializeField] private Board board;

    [SerializeField] private int comboCount = 0;

    public static event Action ONSTARTGAME;
    public static event Action COMBO;
    public static event Action<int> ONCHANGSCORE;

    [SerializeField] private Transform completeUI;
    [SerializeField] private LevelText levelText;

    public bool isNextLevel;

    [SerializeField] private PlayfabLevelScoreManager playfabLevelScoreManager;
    [SerializeField] private DataLocal dataLocal;
    [SerializeField] private bool isVertical;
    [SerializeField] private CandyType currentCandyType;
    
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
        dataLocal = GameObject.Find("DataLoad").GetComponent<DataLocal>();  
    }

    private void OnDestroy()
    {
        PikachuGameLogic.WINGAME -= WinGameCompleteUI;
        LevelTimeManager.OnTimeOut -= LossGameCompleteUI;
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
            // Set CandyType
            currentCandyType = LeverManager.Instance.GetLever(currentLever - 1).type;
        }
        currentScoreInLevel = LeverManager.Instance.GetLever(currentLever - 1).score;
        ONSTARTGAME?.Invoke();
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayInitGame();
        }
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

    public bool GetIsVertical()
    {
        return isVertical;
    }
    public void SetIsVerical(bool isVertical)
    {
        this.isVertical = isVertical;
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
        TotalStar(LevelTimeManager.Instance.Timer,
            LevelTimeManager.Instance.MaxTimer);
        playfabLevelScoreManager.SaveLevelScore(currentLever, scoreGame, currentStar);
        playfabLevelScoreManager.SubmitScore(currentLever, scoreGame);
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySoundWinGame();
        }
    }
    public int GetStarGame()
    {
        TotalStar(LevelTimeManager.Instance.Timer,
            LevelTimeManager.Instance.MaxTimer);
        return currentStar;
    }
    public void TotalStar(float currentTimer, float maxTimer)
    {
        if (currentTimer < 0) currentTimer = 0;
        float percent = currentTimer / maxTimer;
        //Debug.Log("CurrenTime" + currentTimer + " " + "maxTimer" + maxTimer);
        //Debug.Log(percent);
        if (percent >= 0 && percent <= 0.35f)
        {
            currentStar = 1;
        }
        else if (percent > 0.35f && percent <= 0.75)
        {
            currentStar = 2;
        }
        else
        {
            currentStar = 3;
        }
        //Debug.Log(currentStar);
    }
    public void LossGameCompleteUI()
    {
        Debug.Log("LossGame");
        if (board.IsBoardEmpty())
        {
            WinGameCompleteUI();
        }
        else
        {
            completeUI.GetComponent<CompleteUI>().LossGameUI();
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundLoss();
            }
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

    public void SetCandyType(CandyType type)
    {
        currentCandyType = type;
    }

    public CandyType GetCandyType()
    {
        return currentCandyType;
    }

}
