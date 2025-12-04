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
        GameManager.ONSTARTGAME += GetTimer;
    }

    private void OnDestroy()
    {
        GameManager.ONSTARTGAME -= GetTimer;
    }

    private void GetTimer()
    {
        int lever = GameManager.Instance.GetCurrentLever();
        timer = LeverManager.Instance.GetLever(lever - 1).timer;
        maxTimer = timer;
        isRunning = true; 
    }

    private void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;
        timer = Mathf.Max(0, timer);

        OnTimeChanged?.Invoke(timer);

        if (timer <= 0)
        {
            isRunning = false;
            OnTimeOut?.Invoke();
        }
    }
}
