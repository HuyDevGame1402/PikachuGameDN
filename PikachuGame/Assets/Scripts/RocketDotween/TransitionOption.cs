using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class TransitionOption
{
    public float itemScale = 1f;
    public int spawnCount = 1;
    public float spawnInterval = 0.05f; // 20 items/s
    public float duration = 1f;
    public float radiusMultiply = 0;
    public float destroyDelay = 0.1f;
    public Ease movementEase = Ease.InSine;
    public Action playSoundCallback;
    public Action<ItemCurveTransition> onItemSpawnedCallback;
}