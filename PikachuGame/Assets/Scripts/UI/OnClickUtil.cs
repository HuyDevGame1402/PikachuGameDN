using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickUtil : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UtilType type;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(GameUtils.Instance != null && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayOnClickButton();
                GameUtils.Instance.StartUtils(type);
            } 
        }
            
    }
}
