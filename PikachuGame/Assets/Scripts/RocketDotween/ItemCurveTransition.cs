//using UnityEngine;
//using System.Collections;

//public class ItemCurveTransition : MonoBehaviour
//{
//    public Transform jumpLayer;
//    public Transform iconLayer;

//    private void Reset()
//    {
//        jumpLayer = transform.GetChild(0);
//        iconLayer = jumpLayer.GetChild(0);
//    }
//}
using UnityEngine;

public class ItemCurveTransition : MonoBehaviour
{
    public RectTransform rect;
    public RectTransform jumpLayer;
    public RectTransform iconLayer;

    private void Reset()
    {
        rect = GetComponent<RectTransform>();
        jumpLayer = transform.GetChild(0).GetComponent<RectTransform>();
        iconLayer = jumpLayer.GetChild(0).GetComponent<RectTransform>();
    }
}

