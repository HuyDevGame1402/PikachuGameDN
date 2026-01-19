using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class OnClickSetupMusic : MonoBehaviour, IPointerClickHandler
{
    public SoundType type;
    public bool isOnMusic = true;
    public Sprite spriteOnMusic;
    public Sprite spriteOffMusic;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(type == SoundType.Main && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayOnClickButton();
                SoundManager.Instance.SetActiveMusicMain();
            }
            else if(type == SoundType.Background && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayOnClickButton();
                SoundManager.Instance.SetActiveMusicBackground();
            }
            isOnMusic = !isOnMusic;
            if (isOnMusic)
            {
                transform.GetComponent<Image>().sprite = spriteOnMusic;
            }
            else
            {
                transform.GetComponent<Image>().sprite = spriteOffMusic;
            }
        }

    }
}
