//using UnityEngine;

//public class MissileShooter : MonoBehaviour
//{
//    public CurveTransitionDriver curveDriver;
//    public Transform firePoint;
//    public Transform target;

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            Fire();
//        }
//    }

//    void Fire()
//    {
//        curveDriver.instantiateTransition(
//            firePoint.position,
//            target.position
//        );
//    }
//}
using UnityEngine;

public class MissileShooter : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform canvasTransform;   // Canvas ch?a missile
    public RectTransform firePoint;          // UI fire point
    public RectTransform target;             // UI target
    public MissileCurveController missilePrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Spawn missile trong Canvas
        var missile = Instantiate(missilePrefab, canvasTransform);

        // Launch missile
        missile.Launch(
            firePoint.anchoredPosition,
            target.anchoredPosition
        );
    }
}


