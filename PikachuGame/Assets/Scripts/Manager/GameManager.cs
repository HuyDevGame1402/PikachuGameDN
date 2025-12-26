using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int currentLever = 1;
    [SerializeField] private int currentScoreInLevel = 0;
    [SerializeField] private int scoreGame = 0;

    [SerializeField] private Board board;

    [SerializeField] private int comboCount = 0;

    public static event Action ONSTARTGAME;
    public static event Action COMBO;
    public static event Action<int> ONCHANGSCORE;
    protected override void Awake()
    {
        base.Awake();
        board = GameObject.Find("Board").GetComponent<Board>();
    }

    private void Start()
    {
        if(board != null)
        {
            board.GenerateBoard(LeverManager.Instance.GetLever(currentLever - 1));
        }
        currentScoreInLevel = LeverManager.Instance.GetLever(currentLever- 1).score;
        ONSTARTGAME?.Invoke();
    }
    public int GetCurrentLever()
    {
        return currentLever;
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
    public void ResetCombo()
    {
        comboCount = 0;
    }
}
