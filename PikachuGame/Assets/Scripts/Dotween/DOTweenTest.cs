using UnityEngine;
using DG.Tweening;

public class DOTweenTest : MonoBehaviour
{
    RectTransform rect;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartDotween();
        }
    }

    private void StartDotween()
    {
        rect = GetComponent<RectTransform>();

        // reset vị trí ban đầu
        rect.anchoredPosition = new Vector2(-600, 0);

        // chạy animation
        rect.DOAnchorPos(Vector2.zero, 0.6f)
            .SetEase(Ease.OutBack);
    }
}
