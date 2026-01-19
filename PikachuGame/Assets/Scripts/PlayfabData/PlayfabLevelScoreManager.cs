using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

public class PlayfabLevelScoreManager : MonoBehaviour
{
    private const string DATA_KEY = "LevelScores";

    public void SaveLevelScore(int level, int newScore, int stars)
    {
        PlayFabClientAPI.GetUserData(
            new GetUserDataRequest { Keys = new List<string> { DATA_KEY } },
            result =>
            {
                Dictionary<string, LevelScoreData> levelScores =
                    new Dictionary<string, LevelScoreData>();

                if (result.Data != null && result.Data.ContainsKey(DATA_KEY))
                {
                    levelScores = JsonUtility
                        .FromJson<LevelScoreWrapper>(result.Data[DATA_KEY].Value)
                        .ToDictionary();
                }

                string levelKey = level.ToString();

                if (!levelScores.ContainsKey(levelKey) ||
                    newScore > levelScores[levelKey].score
                    || stars != levelScores[levelKey].stars)
                {
                    levelScores[levelKey] = new LevelScoreData
                    {
                        score = newScore,
                        stars = stars
                    };

                    UpdateLevelScores(levelScores);
                }
                else
                {
                    Debug.Log("Score thấp hơn, không lưu");
                }
            },
            OnError
        );
    }

    private void UpdateLevelScores(Dictionary<string, LevelScoreData> levelScores)
    {
        var wrapper = new LevelScoreWrapper(levelScores);

        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { DATA_KEY, JsonUtility.ToJson(wrapper) }
                }
            },
            r => Debug.Log("Save LevelScore success"),
            OnError
        );
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
    public void SubmitScore(int level, int score)
    {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest
            {
                FunctionName = "submitLevelScore",
                FunctionParameter = new
                {
                    level = level,
                    score = score
                }
            },
            result => {
                Debug.Log("Submit score OK");
                Debug.Log("Response: " + result.FunctionResult);  // ← XEM KẾT QUẢ

                // Kiểm tra lỗi CloudScript
                if (result.Error != null)
                {
                    Debug.LogError("CloudScript Error: " + result.Error.Error);
                }
            },
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
}
