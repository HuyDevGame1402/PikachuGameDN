using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int currentLever = 1;
    [SerializeField] private int currentScoreInLevel = 0;
    [SerializeField] private int scoreGame = 0;

    [SerializeField] private Board board;
    [SerializeField] private int comboCount = 0;

    // Firebase variables
    private DatabaseReference dbRef;
    private string databaseUrl = "https://pikachuleaderboard-default-rtdb.asia-southeast1.firebasedatabase.app/";

    public static event Action ONSTARTGAME;
    public static event Action COMBO;
    public static event Action<int> ONCHANGSCORE;

    protected override void Awake()
    {
        base.Awake();
        // Tìm đối tượng Board trong Scene
        GameObject boardObj = GameObject.Find("Board");
        if (boardObj != null) board = boardObj.GetComponent<Board>();

        // Khởi tạo Firebase Database
        dbRef = FirebaseDatabase.GetInstance(databaseUrl).RootReference;
    }

    private void Start()
    {
        if (board != null)
        {
            board.GenerateBoard(LeverManager.Instance.GetLever(currentLever - 1));
        }
        currentScoreInLevel = LeverManager.Instance.GetLever(currentLever - 1).score;
        ONSTARTGAME?.Invoke();
    }

    // --- LOGIC FIREBASE: LƯU ĐIỂM CAO NHẤT ---
    public void EndGameAndSaveFirebase()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null) return;

        string userId = user.UserId;
        DatabaseReference userRef = dbRef.Child("users").Child(userId);

        // Lấy điểm cũ để so sánh High Score
        userRef.Child("score").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int highScoreOnServer = 0;

                if (snapshot.Exists)
                {
                    int.TryParse(snapshot.Value.ToString(), out highScoreOnServer);
                }

                Dictionary<string, object> updates = new Dictionary<string, object>();

                // Chỉ lưu nếu scoreGame hiện tại cao hơn kỷ lục cũ
                if (scoreGame > highScoreOnServer)
                {
                    updates["score"] = scoreGame;
                    Debug.Log("Kỷ lục mới: " + scoreGame);
                }

                updates["level"] = currentLever;
                updates["lastLogin"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                userRef.UpdateChildrenAsync(updates).ContinueWithOnMainThread(updateTask =>
                {
                    if (updateTask.IsCompleted)
                        Debug.Log("Dữ liệu đã được tự động lưu lên Firebase!");
                });
            }
        });
    }

    // --- CÁC HÀM CŨ ---
    public int GetCurrentLever() => currentLever;

    public void SetComboCount()
    {
        comboCount++;
        if (comboCount % 5 == 0)
        {
            COMBO?.Invoke();
        }
    }

    public int GetComboCount() => comboCount;

    public void AddScoreGame()
    {
        scoreGame += comboCount * currentScoreInLevel;
        ONCHANGSCORE?.Invoke(scoreGame);
    }

    public void ResetCombo() => comboCount = 0;
}