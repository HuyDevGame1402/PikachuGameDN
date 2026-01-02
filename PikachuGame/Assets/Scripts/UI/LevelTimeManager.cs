using System;
using UnityEngine;

public class LevelTimeManager : Singleton<LevelTimeManager>
{
    [SerializeField] private float timer;
    public float Timer => timer;

    [SerializeField] private float maxTimer;
    public float MaxTimer => maxTimer;

    public static event Action<float> OnTimeChanged;
    public static event Action OnTimeOut;

    private bool isRunning = false;

    protected override void Awake()
    {
        base.Awake();
        // Đăng ký sự kiện bắt đầu game
        GameManager.ONSTARTGAME += GetTimer;
    }

    private void Start()
    {
        // Kiểm tra bù: Nếu vì lý do nào đó GameManager đã chạy Start trước 
        // thì gọi GetTimer thủ công để kích hoạt đồng hồ
        if (!isRunning && GameManager.Instance != null)
        {
            GetTimer();
        }
    }

    private void OnDestroy()
    {
        GameManager.ONSTARTGAME -= GetTimer;
    }

    private void GetTimer()
    {
        // Lấy level hiện tại từ GameManager
        int lever = GameManager.Instance.GetCurrentLever();

        // Lấy dữ liệu thời gian từ LeverManager
        var levelData = LeverManager.Instance.GetLever(lever - 1);

        if (levelData != null)
        {
            timer = levelData.timer;
            maxTimer = timer;
            isRunning = true;
            Debug.Log($"Level {lever} started. Timer: {timer}s");
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timer = Mathf.Max(0, timer);

            // Gửi dữ liệu cập nhật cho LevelTimeUI
            OnTimeChanged?.Invoke(timer);
        }
        else
        {
            isRunning = false;
            OnTimeOut?.Invoke();

            // TỰ ĐỘNG LƯU ĐIỂM KHI HẾT GIỜ
            if (GameManager.Instance != null)
            {
                Debug.Log("Time's up! Saving score to Firebase...");
                GameManager.Instance.EndGameAndSaveFirebase();
            }
        }
    }
}