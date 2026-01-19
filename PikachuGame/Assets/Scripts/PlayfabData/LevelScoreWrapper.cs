using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelScoreWrapper
{
    private const string DATA_KEY = "LevelScores";
    public List<int> levels = new List<int>();
    public List<LevelScoreData> datas = new List<LevelScoreData>();

    public LevelScoreWrapper(Dictionary<string, LevelScoreData> dict)
    {
        foreach (var kv in dict)
        {
            levels.Add(int.Parse(kv.Key));
            datas.Add(kv.Value);
        }
    }

    public Dictionary<string, LevelScoreData> ToDictionary()
    {
        var dict = new Dictionary<string, LevelScoreData>();
        for (int i = 0; i < levels.Count; i++)
        {
            dict[levels[i].ToString()] = datas[i];
        }
        return dict;
    }
}

[System.Serializable]
public class LevelScoreData
{
    public int score;
    public int stars;   
}

