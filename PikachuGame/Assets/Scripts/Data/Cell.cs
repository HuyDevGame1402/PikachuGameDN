using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    [SerializeField] private int Row;
    [SerializeField] private int Col;
    [SerializeField] private int Id;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Transform backgorundTouched;

    private void Awake()
    {
        backgorundTouched = transform.Find("BackgorundTouched");
    }

    public void Setup(int row, int col, int id, Sprite sprite)
    {
        Row = row;
        Col = col;
        Id = id;
        spriteRenderer.sprite = sprite;
        gameObject.SetActive(Id != -1);
    }

    public bool IsEmpty => Id == -1;

    public void Clear()
    {
        Id = -1;
        gameObject.SetActive(false);
    }

    public int GetRow()
    {
        return Row;
    }
    public int GetCol()
    {
        return Col;
    }
    public int GetId()
    {
        return Id;
    }
    public void ShowBackgroundTouched()
    {
        if (backgorundTouched == null) return;
        backgorundTouched.gameObject.SetActive(true);
    }
    public void HideBackgroundTouched()
    {
        if (backgorundTouched == null) return;
        backgorundTouched.gameObject.SetActive(false);
    }
}
