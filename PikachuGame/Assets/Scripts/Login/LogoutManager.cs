using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LogoutManager : MonoBehaviour
{
    public void Logout()
    {
        // 1. Thực hiện lệnh đăng xuất khỏi Firebase
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log("Đã đăng xuất khỏi Firebase");

        // 2. Chuyển người dùng quay trở lại màn hình Login (LoginScene)
        // Hãy đảm bảo tên Scene trùng khớp với tên trong Build Settings của bạn
        SceneManager.LoadScene("LoginScene");
    }
}