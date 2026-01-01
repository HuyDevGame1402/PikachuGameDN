using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] private int levelChoose;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public int GetLevelChoose()
    {
        return levelChoose;
    }

    public void SetLevelChoose(int levelChoose)
    {
        this.levelChoose = levelChoose;
    }
}
