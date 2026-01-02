using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoginWithEmail : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField EmailField;
    public TMP_InputField PasswordField;
    public TextMeshProUGUI errorText;

    [Header("Loading UI")]
    public GameObject loadingPanel;

    private FirebaseAuth auth;
    private DatabaseReference dbRef;
    private bool isFirebaseReady = false;

    private string databaseUrl = "https://pikachuleaderboard-default-rtdb.asia-southeast1.firebasedatabase.app/";

    void Start()
    {
        if (loadingPanel != null) loadingPanel.SetActive(false);
        if (errorText != null) errorText.text = "";

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                app.Options.DatabaseUrl = new System.Uri(databaseUrl);

                auth = FirebaseAuth.DefaultInstance;
                dbRef = FirebaseDatabase.GetInstance(app, databaseUrl).RootReference;

                isFirebaseReady = true;
                Debug.Log("Firebase Ready!");

                // --- TỰ ĐỘNG ĐĂNG NHẬP ---
                CheckAutoLogin();
            }
            else
            {
                UpdateErrorText("Firebase error!");
            }
        });
    }

    // Kiểm tra xem đã có người dùng đăng nhập từ trước chưa
    private void CheckAutoLogin()
    {
        if (auth.CurrentUser != null)
        {
            Debug.Log("Find remain login: " + auth.CurrentUser.Email);
            GoToGame();
        }
    }

    // --- HÀM ĐĂNG XUẤT (Gọi hàm này từ nút Logout ở màn GameLevelMap) ---
    public void Logout()
    {
        if (auth != null)
        {
            auth.SignOut();
            Debug.Log("Logouted!");
            SceneManager.LoadScene("LoginScene"); // Quay lại màn hình Login
        }
    }

    public void RegisterWithEmailPassword()
    {
        if (!isFirebaseReady) return;
        string email = EmailField.text.Trim();
        string password = PasswordField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            UpdateErrorText("Please enter Email and Password");
            return;
        }

        SetLoading(true);
        UpdateErrorText("Processing.....");

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                SetLoading(false);
                UpdateErrorText("Register failed!");
                return;
            }
            SaveUserIfNotExist(task.Result.User);
        });
    }

    public void LoginWithEmailPassword()
    {
        if (!isFirebaseReady) return;
        string email = EmailField.text.Trim();
        string password = PasswordField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            UpdateErrorText("Please enter Email and password");
            return;
        }

        SetLoading(true);
        UpdateErrorText("Log in....");

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                SetLoading(false);
                UpdateErrorText("Wrong email/password!");
                return;
            }
            SaveUserIfNotExist(task.Result.User);
        });
    }

    private void SaveUserIfNotExist(FirebaseUser user)
    {
        DatabaseReference userRef = dbRef.Child("users").Child(user.UserId);
        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            string currentTime = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            if (task.IsCompleted && !task.Result.Exists)
            {
                Dictionary<string, object> userData = new Dictionary<string, object>
                {
                    ["userId"] = user.UserId,
                    ["email"] = user.Email,
                    ["level"] = 1,
                    ["gold"] = 100,
                    ["lastLogin"] = currentTime
                };
                userRef.UpdateChildrenAsync(userData).ContinueWithOnMainThread(t => GoToGame());
            }
            else
            {
                userRef.Child("lastLogin").SetValueAsync(currentTime).ContinueWithOnMainThread(t => GoToGame());
            }
        });
    }

    private void UpdateErrorText(string message) { if (errorText != null) errorText.text = message; }
    private void SetLoading(bool isLoading) { if (loadingPanel != null) loadingPanel.SetActive(isLoading); }
    private void GoToGame() => SceneManager.LoadScene("GameLevelMap");
}