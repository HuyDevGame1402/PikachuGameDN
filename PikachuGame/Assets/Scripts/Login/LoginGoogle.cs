using UnityEngine;
using Google;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Firebase.Extensions;

public class GoogleLogin : MonoBehaviour
{
    private FirebaseAuth auth;
    private GoogleSignInConfiguration config;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;

        config = new GoogleSignInConfiguration
        {
            WebClientId = "1084900145888-ridjcdktrtf1066809rmr1mdpposht6b.apps.googleusercontent.com",
            RequestIdToken = true,
            UseGameSignIn = false
        };
    }

    void Start()
    {
        if (auth.CurrentUser != null)
        {
            Debug.Log("Đã đăng nhập trước đó → vào GameLevelMap");
            SceneManager.LoadScene("GameLevelMap");
        }
    }

    public void SignInWithGoogle()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("CLICK LOGIN BUTTON");

        GoogleSignIn.Configuration = config;

        GoogleSignIn.DefaultInstance
            .SignIn()
            .ContinueWithOnMainThread(OnGoogleAuthFinished);
#else
        Debug.Log("Google Sign-In chỉ chạy trên Android APK");
#endif
    }

    private void OnGoogleAuthFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsCanceled || task.IsFaulted)
        {
            Debug.LogError("Google Sign-In lỗi");
            return;
        }

        Credential credential =
            GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

        auth.SignInWithCredentialAsync(credential)
            .ContinueWithOnMainThread(OnFirebaseAuthFinished);
    }

    private void OnFirebaseAuthFinished(Task<FirebaseUser> task)
    {
        if (task.IsCanceled || task.IsFaulted)
        {
            Debug.LogError("Firebase Auth lỗi");
            return;
        }

        Debug.Log("🔥 Login thành công → chuyển scene GameLevelMap");
        SceneManager.LoadScene("GameLevelMap");
    }
}
