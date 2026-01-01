using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("ShowLevel");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
