using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class CompleteUI : MonoBehaviour
{
    [SerializeField] private Transform mainUI;
    [SerializeField] private int currentStar = 0;
    [SerializeField] List<Animator> animatorsStar = new List<Animator>();
    [SerializeField] private ScoreText scoreText;
    [SerializeField] private TextMeshProUGUI textStateGame;
    [SerializeField] private Transform imageTitle;
    [SerializeField] private Color gameWinColor;
    [SerializeField] private Color gameLossColor;
    [SerializeField] private Animator animator;

    [SerializeField] private ShowGameUI showGameUI;
    [SerializeField] private PauseUI pauseUI;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void ShowUI()
    {
        for (int i = 0; i < currentStar; i++)
        {
            animatorsStar[i].SetTrigger("Show");
        }
    }

    public void HideUI()
    {
        mainUI.gameObject.SetActive(false);
    }

    public void ShowCompleteUIGame(float currentTimer, float maxTimer)
    {
        if (currentTimer < 0) currentTimer = 0;
        float percent = currentTimer / maxTimer;
        //Debug.Log("CurrenTime" + currentTimer + " " + "maxTimer" + maxTimer);
        //Debug.Log(percent);
        if (percent >= 0 && percent <= 0.35f)
        {
            currentStar = 1;
        }
        else if (percent > 0.35f && percent <= 0.75)
        {
            currentStar = 2;
        }
        else
        {
            currentStar = 3;
        }
        //Debug.Log(currentStar);
    }
    public void SetScoreCompleteUI()
    {
        scoreText.SetScoreText(GameManager.Instance.GetScoreGame());
    }
    public void WinGameUI()
    {
        pauseUI.SetActiveOnClick(false);
        GameManager.Instance.SetGameState(GameState.Victory);
        imageTitle.GetComponent<Image>().color = gameWinColor;
        textStateGame.text = "VICTORY";
        LevelTimeManager.Instance.SetupRunning(false);
        ShowCompleteUIGame(LevelTimeManager.Instance.Timer,
            LevelTimeManager.Instance.MaxTimer);
        SetScoreCompleteUI();
        animator.SetTrigger("Show");
    }
    public void LossGameUI()
    {
        pauseUI.SetActiveOnClick(false);
        GameManager.Instance.SetGameState(GameState.GameOver);
        imageTitle.GetComponent<Image>().color = gameLossColor;
        textStateGame.text = "GAME OVER";
        LevelTimeManager.Instance.SetupRunning(false);
        ShowCompleteUIGame(LevelTimeManager.Instance.Timer,
            LevelTimeManager.Instance.MaxTimer);
        SetScoreCompleteUI();
    }
    public void OnClickNextLevel()
    {
        pauseUI.SetActiveOnClick(true);
        GameManager.Instance.isNextLevel = true;
        animator.SetTrigger("Hide");
    }
    public void LoadingGame()
    {
        pauseUI.SetActiveOnClick(true);
        showGameUI.Hide();
    }
    public void LoadHome()
    {
        SceneManager.LoadScene("GameLevelMap");
    }
}
