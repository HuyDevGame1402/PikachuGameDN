using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    None,
    Background,
    Main
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource audioWin;
    [SerializeField] private AudioSource audioLoss;
    [SerializeField] private AudioSource audioOnClickSetting;
    [SerializeField] private AudioSource audioOnClickButton;
    [SerializeField] private AudioSource audioOnClickLevel;


    [SerializeField] private AudioSource audioOnStartLevel;
    [SerializeField] private AudioSource audioPieceCrash;
    [SerializeField] private AudioSource audioRocket;
    [SerializeField] private AudioSource audioSwap;
    [SerializeField] private AudioSource audioClickButton_1;

    [SerializeField] private AudioSource audioBackground;

    [SerializeField] private bool isActiveMusicBackground;
    [SerializeField] private bool isActiveMusicMain;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        DontDestroyOnLoad(this);

        PlaySoundBackground();
    }
    public void PlaySoundBackground()
    {
        if (!isActiveMusicBackground) return;
        audioBackground.Play();
    }
    public void PlaySoundWinGame()
    {
        if (!CheckActiveMusic()) return;
        audioWin.Play();
    }
    public void PlaySoundLoss()
    {
        if (!CheckActiveMusic()) return;
        audioLoss.Play(); 
    }

    public void PlaySoundOnClickSetting()
    {
        if (!CheckActiveMusic()) return;
        audioOnClickSetting.Play();
    }

    public void PlayOnClickButton()
    {
        if (!CheckActiveMusic()) return;
        audioOnClickButton.Play();
    }
    public void PlayOnClickLevel()
    {
        if (!CheckActiveMusic()) return;
        audioOnClickLevel.Play();
    }
    public void PlayOnClickButton_1()
    {
        if (!CheckActiveMusic()) return;
        audioClickButton_1.Play();
    }
    public void PlayRocket()
    {
        if (!CheckActiveMusic()) return;
        audioRocket.Play();
    }
    public void PlayPieceCrack()
    {
        if (!CheckActiveMusic()) return;
        audioPieceCrash.Play();
    }
    public void PlayInitGame()
    {
        if (!CheckActiveMusic()) return;
        audioOnStartLevel.Play();
    }
    public void PlaySwap()
    {
        if (!CheckActiveMusic()) return;
        audioSwap.Play();
    }

    public void SetActiveMusicBackground()
    {
        isActiveMusicBackground = !isActiveMusicBackground;
    }
    public void SetActiveMusicMain()
    {
        isActiveMusicMain = !isActiveMusicMain;
    }

    private bool CheckActiveMusic()
    {
        if (isActiveMusicMain)
        {
            return true;
        }
        return false;
    }
}
