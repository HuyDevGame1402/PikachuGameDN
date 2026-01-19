using DG.Tweening;
using UnityEngine;

public class MissileCurveController : MonoBehaviour
{
    public RectTransform rect;
    public float curveHeight = 200f;
    public float curveDuration = 0.8f;
    public float finalDuration = 0.3f;

    private Tween moveTween;

    public void Launch(Vector2 from, Vector2 target)
    {
        rect.anchoredPosition = from;

        // ?? T?o ?i?m cong
        Vector2 mid = (from + target) * 0.5f;
        mid += Vector2.Perpendicular(target - from).normalized * curveHeight;

        // ?? Phase 1: bay cong
        moveTween = rect.DOPath(
            new Vector3[] { from, mid, target },
            curveDuration,
            PathType.CatmullRom
        )
        .SetEase(Ease.OutSine)
        .OnComplete(() =>
        {
            // ?? Phase 2: ?âm th?ng
            rect.DOAnchorPos(target, finalDuration)
                .SetEase(Ease.InQuad);
        });
    }

    private void OnDisable()
    {
        moveTween?.Kill();
    }
}
