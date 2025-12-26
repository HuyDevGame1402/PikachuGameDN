using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private void Start()
    {
        GameManager.ONCHANGSCORE += UpdateScoreUiGame;
        scoreText = transform.GetComponent<TextMeshProUGUI>();
    }

    private void UpdateScoreUiGame(int score)
    {
        if (scoreText == null) return;
        scoreText.text = score.ToString();
    }
}
