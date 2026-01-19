//using DG.Tweening;
//using System;
//using System.Collections;
//using UnityEngine;

//public class CurveTransitionDriver : MonoBehaviour
//{

//    public float fxRadius = 0;
//    public ItemCurveTransition prefabItem;

//    public void instantiateTransition(Vector3 fromPos, Vector3 toPos, TransitionOption fxOption = null, Action onComplete = null)
//    {
//        if (fxOption == null)
//        {
//            fxOption = new TransitionOption();
//        }
//        fxOption.playSoundCallback?.Invoke();
//        var spawnCount = fxOption.spawnCount;

//        for (int i = 0; i < spawnCount; i++)
//        {
//            var index = i;
//            DOVirtual.DelayedCall(fxOption.spawnInterval * index, delegate {
//                var newIcon = Instantiate(prefabItem, transform);
//                newIcon.transform.position = fromPos;
//                newIcon.transform.localScale = newIcon.transform.localScale * fxOption.itemScale;
//                fxOption.onItemSpawnedCallback?.Invoke(newIcon);
//                if (fxOption.radiusMultiply != 0 && fxRadius != 0)
//                {
//                    newIcon.transform.localEulerAngles = Vector3.forward * UnityEngine.Random.Range(0, 360f);
//                    newIcon.jumpLayer.DOLocalMoveY(fxOption.radiusMultiply * fxRadius * UnityEngine.Random.Range(0.5f, 1f), fxOption.duration / 2f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
//                }
//                newIcon.transform.DOMove(toPos, fxOption.duration).SetEase(fxOption.movementEase).OnComplete(delegate {
//                    Destroy(newIcon.gameObject, fxOption.destroyDelay);
//                    if (index == spawnCount - 1)
//                    {
//                        onComplete?.Invoke();
//                    }
//                });
//            }).SetLink(gameObject);
//        }
//    }

//}

using DG.Tweening;
using System;
using UnityEngine;

public class CurveTransitionDriver : MonoBehaviour
{
    public float fxRadius = 100f;
    public ItemCurveTransition prefabItem;

    public void instantiateTransition(
        Vector3 fromPos,
        Vector3 toPos,
        TransitionOption fxOption = null,
        Action onComplete = null)
    {
        if (fxOption == null)
            fxOption = new TransitionOption();

        fxOption.playSoundCallback?.Invoke();

        var spawnCount = fxOption.spawnCount;

        for (int i = 0; i < spawnCount; i++)
        {
            int index = i;

            DOVirtual.DelayedCall(fxOption.spawnInterval * index, () =>
            {
                var newIcon = Instantiate(prefabItem, transform);
                newIcon.rect.anchoredPosition = fromPos;
                newIcon.rect.localScale *= fxOption.itemScale;

                fxOption.onItemSpawnedCallback?.Invoke(newIcon);

                // ?? CONG ? ?ÂY
                if (fxOption.radiusMultiply != 0 && fxRadius > 0)
                {
                    float dir = UnityEngine.Random.value > 0.5f ? 1f : -1f;
                    float height = fxRadius * fxOption.radiusMultiply * dir;

                    newIcon.jumpLayer.DOAnchorPosY(
                        height,
                        fxOption.duration / 2f
                    )
                    .SetEase(Ease.OutQuad)
                    .SetLoops(2, LoopType.Yoyo);
                }

                // ?? BAY T?I TARGET (UI)
                newIcon.rect.DOAnchorPos(
                    toPos,
                    fxOption.duration
                )
                .SetEase(fxOption.movementEase)
                .OnComplete(() =>
                {
                    Destroy(newIcon.gameObject, fxOption.destroyDelay);

                    if (index == spawnCount - 1)
                        onComplete?.Invoke();
                });

            }).SetLink(gameObject);
        }
    }
}

