using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : Singleton<LeverManager>
{
    [SerializeField] private List<LevelData> levelDatas = new List<LevelData>();

    protected override void Awake()
    {
        base.Awake();
    }

    public LevelData GetLever(int level)
    {
        if(level < 0 && level >= levelDatas.Count) return null;
        return levelDatas[level];
    }
}
