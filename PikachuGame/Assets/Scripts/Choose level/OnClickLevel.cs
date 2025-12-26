using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickLevel : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public bool isActive;

    public void SetActive(bool active)
    {
        isActive = active;
    }

    private void OnMouseDown()
    {
        if (animator == null || !isActive) return;
        animator.SetTrigger("Hide");
    }
}
