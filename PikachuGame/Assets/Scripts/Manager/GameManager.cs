using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int currentLever = 1;

    [SerializeField] private Board board;

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
    }

}
