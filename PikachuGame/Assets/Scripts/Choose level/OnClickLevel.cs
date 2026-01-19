using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickLevel : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int level;
    public int LEVEL => level;

    public bool isActive;

    public void SetActive(bool active)
    {
        isActive = active;
    }

    private void OnMouseDown()
    {
        if (animator == null || !isActive) return;
        SoundManager.Instance.PlayOnClickLevel();
        GameObject.Find("GameData").GetComponent<GameData>().SetLevelChoose(level);
        animator.SetTrigger("Hide");
    }
}
