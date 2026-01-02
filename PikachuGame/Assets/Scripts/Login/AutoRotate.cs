using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float speed = 250f;

    void Update()
    {
        // Xoay quanh trục Z theo thời gian thực
        transform.Rotate(0, 0, -speed * Time.deltaTime);
    }
}