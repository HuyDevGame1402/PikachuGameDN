//using System;
//using System.Collections;
//using UnityEngine;

//public class ForwardDirectionTracker2D : MonoBehaviour
//{

//    private Vector2 lastPosition;

//    private void OnEnable()
//    {
//        StartCoroutine(trackForwardDirection());
//    }

//    private IEnumerator trackForwardDirection()
//    {
//        while (true)
//        {
//            var newPosition = (Vector2)transform.position;
//            var diff = newPosition - lastPosition;
//            if (diff != Vector2.zero)
//            {
//                transform.eulerAngles = -Vector3.forward * Vector2.SignedAngle(diff, Vector2.up);
//                lastPosition = newPosition;
//            }
//            yield return null;
//        }
//    }
//}

using UnityEngine;
using System.Collections;

public class ForwardDirectionTracker2D : MonoBehaviour
{
    private Vector2 lastPosition;

    private void OnEnable()
    {
        lastPosition = transform.position;
        StartCoroutine(trackForwardDirection());
    }

    private IEnumerator trackForwardDirection()
    {
        while (true)
        {
            var newPosition = (Vector2)transform.position;
            var diff = newPosition - lastPosition;

            if (diff.sqrMagnitude > 0.0001f)
            {
                float angle = Vector2.SignedAngle(Vector2.up, diff);
                transform.rotation = Quaternion.Euler(0, 0, angle);
                lastPosition = newPosition;
            }

            yield return null;
        }
    }
}

