using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YPositionSorting : MonoBehaviour
{
    public int baseSortingOrder = 5000;
    public float yMultiplier = 100f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        SetupLayer();
    }
    private void SetupLayer()
    {
        int sortingOrder = baseSortingOrder - Mathf.RoundToInt(transform.position.y * yMultiplier);
        sr.sortingOrder = sortingOrder;
    }
}
