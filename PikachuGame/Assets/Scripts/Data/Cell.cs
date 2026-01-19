using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Cell : MonoBehaviour
{
    [SerializeField] private int Row;
    [SerializeField] private int Col;
    [SerializeField] private int Id;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Transform backgorundTouched;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private SpriteRenderer spriteBackground;
    private Coroutine highlightCoroutine;
    public float highlightDuration = 3f;
    public float blinkInterval = 0.25f;

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
        spriteRenderer.enabled = false;
        gameObject.SetActive(Id != -1);
    }

    public void SetupRowAndCol(int row, int col)
    {
        Row = row;
        Col = col;
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
    public Vector2Int GetVector2RowAndCol()
    {
        return new Vector2Int(Row, Col);
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
    public void SetActiveSprite(bool active)
    {
        spriteRenderer.enabled = active;
    }
    public void Highlight()
    {
        if (highlightCoroutine != null)
            StopCoroutine(highlightCoroutine);

        highlightCoroutine = StartCoroutine(HighlightRoutine());
    }

    IEnumerator HighlightRoutine()
    {
        float timer = 0f;
        bool isHighlight = true;

        while (timer < highlightDuration)
        {
            spriteBackground.color = isHighlight ? highlightColor : normalColor;
            isHighlight = !isHighlight;

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }
        spriteBackground.color = normalColor;
        highlightCoroutine = null;
    }
}
