using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGameUI : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }
    public void LoadGame()
    {
        GameManager.Instance.LoadGameLogic();
    }
}
