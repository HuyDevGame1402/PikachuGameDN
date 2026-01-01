using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
}
