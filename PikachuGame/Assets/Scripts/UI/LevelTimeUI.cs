using UnityEngine;
using TMPro;

public class LevelTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private Transform fillTransformTimer;

    private void OnEnable()
    {
        LevelTimeManager.OnTimeChanged += UpdateTimerUI;
    }

    private void OnDisable()
    {
        LevelTimeManager.OnTimeChanged -= UpdateTimerUI;
    }

    private void UpdateTimerUI(float time)
    {
        if (textTimer == null || fillTransformTimer == null) return;

        // TextMeshProGUI
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        textTimer.text = $"{minutes:00}:{seconds:00}";

        // Scale Fill Transform
        Vector3 fillScale = fillTransformTimer.transform.localScale;
        fillScale.x = time / LevelTimeManager.Instance.MaxTimer;
        fillTransformTimer.transform.localScale = fillScale;

    }
}
