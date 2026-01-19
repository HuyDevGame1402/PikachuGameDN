using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataLocal : MonoBehaviour
{
    private const string DATA_KEY = "LevelScores";
    public Dictionary<string, LevelScoreData> levelScores = new Dictionary<string, LevelScoreData>();
    public string displayName = "";
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetLevelData()
    {
        PlayFabClientAPI.GetUserData(
            new GetUserDataRequest { Keys = new List<string> { DATA_KEY } },
            result =>
            {

                if (result.Data != null && result.Data.ContainsKey(DATA_KEY))
                {
                    levelScores = JsonUtility
                        .FromJson<LevelScoreWrapper>(result.Data[DATA_KEY].Value)
                        .ToDictionary();
                }
                SceneManager.LoadScene("GameLevelMap");

            },
            OnError
        );
    }
    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
