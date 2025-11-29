using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Board : MonoBehaviour
{
    [SerializeField] private int Rows = 5;
    [SerializeField] private int Cols = 6;
    [SerializeField] private int TypeCount = 12; // số loại hình

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform boardRoot;

    private Cell[,] Cells;       // tham chiếu tới prefab spawn
    private int[,] Matrix;       // ma trận logic ID hình

    [SerializeField] private List<int> listTest = new List<int>();

    [SerializeField] private Dictionary<int , List<Vector2Int>> groups = new Dictionary<int , List<Vector2Int>>();

    [SerializeField] private float offsetCell = 1.2f;
    [SerializeField] private float startPosx;
    [SerializeField] private float startPosy;

    [SerializeField] private bool checkGameLogic;

    public void GenerateBoard(LevelData level)
    {
        if (level != null)
        {
            Rows = level.Rows;
            Cols = level.Cols;
            TypeCount = level.TypeCount;
        }

        Cells = new Cell[Rows, Cols];
        Matrix = new int[Rows, Cols];

        int total = Rows * Cols;
        List<int> localList = ListIdInBoard(total, TypeCount);
        // ----- Spawn CellPrefab -----
        float centerOffsetX = (Cols - 1) * offsetCell / 2f;

        while (!checkGameLogic)
        {
            List<int> listId = localList;
            groups.Clear();
            ClearMatrix(Rows,Cols);
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    float posX = c * offsetCell - centerOffsetX;
                    float posY = startPosy - r * offsetCell;
                    int value = Random.Range(0, total);
                    int id = listId[value];
                    Matrix[r, c] = id;
                    GameObject newCell = Instantiate(cellPrefab, new Vector3(posX, posY, 0), Quaternion.identity, boardRoot);
                    if (SpriteManager.Instance.GetSprite(id) != null)
                    {
                        newCell.GetComponent<Cell>().Setup(r, c, id, SpriteManager.Instance.GetSprite(id));
                    }
                    total -= 1;


                    if (!groups.ContainsKey(id))
                    {
                        List<Vector2Int> posList = new List<Vector2Int>();
                        posList.Add(new Vector2Int(r, c));
                        groups.Add(id, posList);
                    }
                    else
                    {
                        List<Vector2Int> posList = groups[id];
                        posList.Add(new Vector2Int(r, c));
                        groups[id] = posList;
                    }

                    listId.Remove(id);
                }
            }

            int[,] matrixPadding = PikachuGameLogic.Instance.GetPaddedMatrix(Matrix);

            foreach (var group in groups)
            {
                List<Vector2Int> posList = group.Value;

                if (checkGameLogic) break;

                for (int i = 0; i < posList.Count - 1; i++)
                {
                    if (!checkGameLogic)
                    {
                        for (int j = i + 1; j < posList.Count; j++)
                        {
                            if (!checkGameLogic)
                            {
                                Vector2Int posA = posList[i];
                                Vector2Int posB = posList[j];
                                checkGameLogic = PikachuGameLogic.Instance.GetCanConnect(matrixPadding,
                                    posA.x, posA.y, posB.x, posB.y);
                            }
                        }
                    }
                }
            }
        }
    }

    public int[,] GetMatrix()
    {
        return Matrix;
    }

    public void SetCellEmpty(int row, int col)
    {
        Matrix[row, col] = -1;
    }
    private void ClearMatrix(int row, int col)
    {
        for(int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Matrix[i, j] = -1;
            }
        }
    }
    private List<int> ListIdInBoard(int total, int typeCount)
    {
        List<int> listId = new List<int>();

        int a = total / (typeCount * 2); // a ứng với số lần lặp qua tổng cặp
        int b = (total % (typeCount * 2)) / 2; // b ứng với số cặp còn khuyết thiếu còn lại

        if(a != 0)
        {
            for(int i = 0; i < a; i++)
            {
                for(int j = 0; j < typeCount; j++)
                {
                    listId.Add(j);
                    listId.Add(j);
                }
            }
        }

        if(b != 0)
        {
            for(int i = 0; i < b; i++)
            {
                listId.Add(i);
                listId.Add(i);
            }
        }

        return listId;
    }
}