using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoginCandyGame : MonoBehaviour
{
    public float moveSpeed = 5f;

    /// <summary>
    /// ms = 1 : xuống
    /// ms = 2 : ngang (trái / phải)
    /// directionX : -1 = trái, 1 = phải (chỉ dùng khi ms = 2)
    /// </summary>
    /// 

    [SerializeField] private Board board;
    [SerializeField] private Transform boardRoot;

    [SerializeField] private List<GameObject> listCell = new List<GameObject>();


    private void Start()
    {
        if(PikachuGameLogic.Instance != null)
        {
            PikachuGameLogic.STARTLOGICCANDY += FindAndMoveToCell;
        }
    }
    private void OnDestroy()
    {
        if (PikachuGameLogic.Instance != null)
        {
            PikachuGameLogic.STARTLOGICCANDY -= FindAndMoveToCell;
        }
    }

    //private IEnumerator MoveToCell(Transform cellTransform, Vector3 targetPos)
    //{
    //    Vector3 startPos = cellTransform.position;
    //    float t = 0f;

    //    while (t < 1f)
    //    {
    //        t += Time.deltaTime * moveSpeed;
    //        cellTransform.position = Vector3.Lerp(startPos, targetPos, t);
    //        yield return null;
    //    }

    //    cellTransform.position = targetPos;
    //}

    public void MoveToCell(Transform cellTransform, Vector3 targetPos)
    {
        cellTransform.DOKill(); // tránh chồng tween

        float dropTime = 0.25f;
        float bounceUp = 0.12f;   // độ nảy (đơn vị world)
        float bounceUpTime = 0.08f;
        float bounceDownTime = 0.06f;

        Sequence seq = DOTween.Sequence();

        // 1️⃣ Rơi xuống
        seq.Append(
            cellTransform.DOMove(targetPos, dropTime)
                         .SetEase(Ease.OutQuad)
        );

        // 2️⃣ Nảy lên nhẹ
        seq.Append(
            cellTransform.DOMoveY(targetPos.y + bounceUp, bounceUpTime)
                         .SetEase(Ease.OutQuad)
        );

        // 3️⃣ Trở về vị trí chuẩn
        seq.Append(
            cellTransform.DOMoveY(targetPos.y, bounceDownTime)
                         .SetEase(Ease.InQuad)
        );
    }

    public void FindAndMoveToCell(int row, int col, CandyType candyType)
    {
        bool isVertical = false;
        if(candyType == CandyType.Vertical)
        {
            isVertical = true;
        }
        else
        {
            isVertical = false;
        }
        int[,] matrix = board.GetMatrix();
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        List<GameObject> cells = new List<GameObject>();


        if (!isVertical)
        {
            for (int i = 0; i < rows; i++)
            {
                if(i == row)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Debug.Log(matrix[i, j]);
                        if(j >= col + 1 && matrix[i,j] != -1)
                        {
                            GameObject cellObject = FindCellInBoard(i, j);
                            if(cellObject != null)
                            {
                                listCell.Add(cellObject);
                                cells.Add(cellObject);
                            }
                        }
                    }
                }
                
            }
            Vector3 startPositionTarget = board.GetPositionMatrix(row, col);
            int colOld = col;
            //Debug.Log("===========Start Logic Candy============" + row);
            //Debug.Log(row);
            for (int i = 0; i < cells.Count; i++)
            {
                //Debug.Log(colOld + "row: " + row);
                //StartCoroutine(MoveToCell(cells[i].transform, startPositionTarget));
                MoveToCell(cells[i].transform, startPositionTarget);
                board.SetIdInMatrix(row, colOld, cells[i].GetComponent<Cell>().GetId());
                board.SetCellEmpty(cells[i].GetComponent<Cell>().GetRow(), cells[i].GetComponent<Cell>().GetCol());
                cells[i].GetComponent<Cell>().SetupRowAndCol(row, colOld);
                colOld += 1;
                startPositionTarget = board.GetPositionMatrix(row, colOld);
            }
            //Debug.Log("===========End Logic Candy============"  + row);


            //int[,] matrixNew = board.GetMatrix();
            //Debug.Log("===========Start Value Matrix============" + row);
            //for(int i = 0; i < rows; i ++)
            //{
            //    for(int j = 0; j < cols; j ++)
            //    {
            //        if(i == row)
            //        {
            //            Debug.Log(matrixNew[i,j]);
            //        }
            //    }
            //}
            //Debug.Log("===========End Value Matrix============" + row);
        }
        else
        {
            for (int j = 0; j < cols; j++)
            {
                if (j == col)
                {
                    for (int i = rows - 1; i >= 0 ; i--)
                    {
                        if (i <= row - 1 && matrix[i, j] != -1)
                        {
                            GameObject cellObject = FindCellInBoard(i, j);
                            if (cellObject != null)
                            {
                                listCell.Add(cellObject);
                                cells.Add(cellObject);
                            }
                        }
                    }
                }

            }
            Vector3 startPositionTarget = board.GetPositionMatrix(row, col);
            int rowOld = row;
            //Debug.Log("===========Start Logic Candy============" + row);
            //Debug.Log(row);
            for (int i = 0; i < cells.Count; i++)
            {
                //Debug.Log(colOld + "row: " + row);
                //StartCoroutine(MoveToCell(cells[i].transform, startPositionTarget));
                MoveToCell(cells[i].transform, startPositionTarget);
                board.SetIdInMatrix(rowOld, col, cells[i].GetComponent<Cell>().GetId());
                board.SetCellEmpty(cells[i].GetComponent<Cell>().GetRow(), cells[i].GetComponent<Cell>().GetCol());
                cells[i].GetComponent<Cell>().SetupRowAndCol(rowOld, col);
                rowOld -= 1;
                startPositionTarget = board.GetPositionMatrix(rowOld, col);
            }
        }
    }
    private GameObject FindCellInBoard(int row, int col)
    {
        for(int i = 0; i < boardRoot.childCount; i++)
        {
            Cell cell = boardRoot.GetChild(i).GetComponent<Cell>();
            if(cell.GetRow() == row && cell.GetCol() == col)
            {
                return boardRoot.GetChild(i).gameObject;
            }
        }
        return null;
    }

}
