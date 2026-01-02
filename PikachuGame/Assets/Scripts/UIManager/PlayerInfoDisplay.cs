using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class PlayerInfoDisplay : MonoBehaviour
{
    [Header("Kéo đối tượng Text (TMP) bên trong Image vào đây")]
    public TextMeshProUGUI scoreDisplayText;

    private DatabaseReference dbRef;
    private string databaseUrl = "https://pikachuleaderboard-default-rtdb.asia-southeast1.firebasedatabase.app/";

    void Start()
    {
        dbRef = FirebaseDatabase.GetInstance(databaseUrl).RootReference;
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user != null)
        {
            LoadScore(user.UserId);
        }
    }

    private void LoadScore(string userId)
    {
        // Truy cập vào dữ liệu người dùng
        dbRef.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // Lấy giá trị score, nếu không có thì mặc định là 0
                    string currentScore = snapshot.Child("score").Value?.ToString() ?? "0";

                    // Hiển thị lên UI
                    if (scoreDisplayText != null)
                    {
                        scoreDisplayText.text = currentScore;
                    }
                }
            }
        });
    }
}