using UnityEngine;

public class CameraVerticalDrag : MonoBehaviour
{
    [Header("Settings")]
    public float dragSpeed = 0.02f;
    public float minY = -2f;
    public float maxY = 8.5f;

    private Vector3 startMousePos;
    private float startCamY;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = Input.mousePosition;
            startCamY = transform.position.y;
        }

        if (Input.GetMouseButton(0))
        {
            float deltaY = Input.mousePosition.y - startMousePos.y;

            float newY = startCamY + (-deltaY) * dragSpeed;
            newY = Mathf.Clamp(newY, minY, maxY);

            transform.position = new Vector3(
                transform.position.x,
                newY,
                transform.position.z
            );
        }
    }
}
