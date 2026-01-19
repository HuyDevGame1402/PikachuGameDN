using UnityEngine;
using DG.Tweening;

public class ButtonTween : MonoBehaviour
{
    public void Pop()
    {
        transform
            .DOScale(1.2f, 0.15f)
            .SetLoops(2, LoopType.Yoyo);
    }
}
