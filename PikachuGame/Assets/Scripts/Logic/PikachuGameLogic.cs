using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PikachuGameLogic : Singleton<PikachuGameLogic>
{
    [SerializeField] private Cell selectedCellA;
    [SerializeField] private Cell selectedCellB;

    [SerializeField] private Board board;

    [SerializeField] private bool isProcessingLogic;

    protected override void Awake()
    {
        base.Awake();
        board = GameObject.Find("Board").GetComponent<Board>();
    }

    public void SetSelectedCell(Cell cell)
    {
        if (cell == null) return;

        if(selectedCellA != null && selectedCellA == cell) return;

        if (selectedCellA == null)
        {
            selectedCellA = cell;
            return;
        }

        selectedCellB = cell;
        isProcessingLogic = true;
        StartCoroutine(DelayProcessGameLogic());
    }

    public bool GetProcessLogic()
    {
        return isProcessingLogic;
    }

    private IEnumerator DelayProcessGameLogic()
    {
        yield return new WaitForSeconds(0.5f);
        LogicGame(selectedCellA, selectedCellB);
    }

    private void LogicGame(Cell cellA, Cell cellB)
    {
        if (cellA.GetId() != cellB.GetId())
        {
            Debug.Log("false");
            HideBackGroundTouch();
            ResetSelectedCell();
            isProcessingLogic = false;
            return;
        }

        // Lấy matrix có padding xung quanh
        int[,] matrix = GetPaddedMatrix(board.GetMatrix());

        int rA = cellA.GetRow() + 1;
        int cA = cellA.GetCol() + 1;
        int rB = cellB.GetRow() + 1;
        int cB = cellB.GetCol() + 1;

        bool canConnect = CanConnect(matrix, rA, cA, rB, cB);

        if (canConnect)
        {
            Debug.Log("true");
            // Khi ăn được, đặt -1 vào matrix gốc
            ProcessingConnect(rA, rB, cA, cB);
        }
        else
        {
            Debug.Log("false");
            HideBackGroundTouch();
        }

        // Reset state
        ResetSelectedCell();
        isProcessingLogic = false;
    }

    private void HideBackGroundTouch()
    {
        selectedCellA.HideBackgroundTouched();
        selectedCellB.HideBackgroundTouched();
    }

    private void ResetSelectedCell()
    {
        selectedCellA = null;
        selectedCellB = null;
    }

    private void ProcessingConnect(int rA, int rB, int cA, int cB)
    {
        board.SetCellEmpty(rA - 1, cA - 1);
        board.SetCellEmpty(rB - 1, cB - 1);
        Vector3 posCellA = selectedCellA.transform.position;
        Vector3 posCellB = selectedCellB.transform.position;
        Destroy(selectedCellA.gameObject);
        Destroy(selectedCellB.gameObject);
        int idVFX = 0;

        SpawnVfx(VFXManager.Instance.GetVFX(idVFX), posCellA, idVFX);
        SpawnVfx(VFXManager.Instance.GetVFX(idVFX), posCellB, idVFX);

    }

    private void SpawnVfx(GameObject vfx, Vector3 pos, int idVFX)
    {
        if (IsPrefab(vfx))
        {
            GameObject vfxInGame = Instantiate(vfx, pos, Quaternion.identity);
            ObjectPool.Instance.AddVfxDic(idVFX, vfxInGame);
        }
        else
        {
            vfx.transform.position = pos;
            vfx.SetActive(true);
        }
    }

    private bool CanConnect(int[,] matrix, int rA, int cA, int rB, int cB)
    {
        // 0 góc
        if (rA == rB && IsClearHorizontal(matrix, rA, cA, cB)) return true;
        if (cA == cB && IsClearVertical(matrix, cA, rA, rB)) return true;

        // 1 góc
        if (IsEmpty(matrix, rA, cB) && IsClearHorizontal(matrix, rA, cA, cB) && IsClearVertical(matrix, cB, rA, rB))
            return true;

        if (IsEmpty(matrix, rB, cA) && IsClearVertical(matrix, cA, rA, rB) && IsClearHorizontal(matrix, rB, cA, cB))
            return true;

        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        // 2 góc theo hàng
        for (int r = 0; r < rows; r++)
        {
            if (IsEmpty(matrix, r, cA) && IsEmpty(matrix, r, cB) &&
                IsClearVertical(matrix, cA, Mathf.Min(rA, r), Mathf.Max(rA, r)) &&
                IsClearHorizontal(matrix, r, cA, cB) &&
                IsClearVertical(matrix, cB, Mathf.Min(rB, r), Mathf.Max(rB, r)))
                return true;
        }

        // 2 góc theo cột
        for (int c = 0; c < cols; c++)
        {
            if (IsEmpty(matrix, rA, c) && IsEmpty(matrix, rB, c) &&
                IsClearHorizontal(matrix, rA, Mathf.Min(cA, c), Mathf.Max(cA, c)) &&
                IsClearVertical(matrix, c, Mathf.Min(rA, rB), Mathf.Max(rA, rB)) &&
                IsClearHorizontal(matrix, rB, Mathf.Min(cB, c), Mathf.Max(cB, c)))
                return true;
        }

        return false;
    }

    public bool GetCanConnect(int[,] matrix, int rA, int cA, int rB, int cB)
    {
        return CanConnect(matrix, rA, cA, rB, cB);
    }

    public int[,] GetPaddedMatrix(int[,] original)
    {
        int rows = original.GetLength(0);
        int cols = original.GetLength(1);

        int[,] padded = new int[rows + 2, cols + 2];

        // Khởi tạo rỗng -1
        for (int r = 0; r < rows + 2; r++)
            for (int c = 0; c < cols + 2; c++)
                padded[r, c] = -1;

        // Copy dữ liệu gốc vào giữa
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                padded[r + 1, c + 1] = original[r, c];

        return padded;
    }

    private bool IsEmpty(int[,] matrix, int r, int c)
    {
        return matrix[r, c] == -1;
    }

    private bool IsClearHorizontal(int[,] matrix, int row, int col1, int col2)
    {
        int start = Mathf.Min(col1, col2) + 1;
        int end = Mathf.Max(col1, col2);
        for (int c = start; c < end; c++)
            if (matrix[row, c] != -1) return false;
        return true;
    }

    private bool IsClearVertical(int[,] matrix, int col, int row1, int row2)
    {
        int start = Mathf.Min(row1, row2) + 1;
        int end = Mathf.Max(row1, row2);
        for (int r = start; r < end; r++)
            if (matrix[r, col] != -1) return false;
        return true;
    }
    bool IsPrefab(GameObject obj)
    {
        return !obj.scene.IsValid();
    }

}

