using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Transform pauseUI;
    [SerializeField] private ShowGameUI showGameUI;

    [SerializeField] private bool isActiveOnClick = true;
    public void ShowUI()
    {
        if (!isActiveOnClick)
        {
            return;
        }
        pauseUI.gameObject.SetActive(true);
        GameManager.Instance.SetGameState(GameState.Paused);
        LevelTimeManager.Instance.SetupRunning(false);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayOnClickButton();
        }
    }
    public void HideUI()
    {
        pauseUI.gameObject.SetActive(false);
        GameManager.Instance.SetGameState(GameState.Playing);
        LevelTimeManager.Instance.SetupRunning(true);
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayOnClickButton();
        }
    }

    public void NewGame()
    {
        GameManager.Instance.isNextLevel = false;
        HideUI();
        showGameUI.Hide();
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayOnClickButton();
        }
    }
    public void QuitGame()
    {
        LoadGameScene();
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayOnClickButton();
        }
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameLevelMap");
    }
    public void SetActiveOnClick(bool active)
    {
        isActiveOnClick = active;
    }
}
