using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public enum UtilType
{
    Rocket,
    Find,
    Swap,
    Time
}

public class GameUtils : Singleton<GameUtils>
{
    [Header("Rocket")]
    [SerializeField] private GameObject iconRocket;
    [SerializeField] private GameObject rocketPrefab;
    public int rocketCompelteCount = 0;

    [SerializeField] private float distanceFromCamera = 10;

    [Header("Board")]
    [SerializeField] private Transform boardRoot;
    [SerializeField] private Transform board;

    [Header("Timer")]
    [SerializeField] private float timer;


    protected override void Awake()
    {
        base.Awake();
    }

    public void ShootRocket()
    {
        rocketCompelteCount = 0;
        Vector3 posSpawn = ConvertPosSceneToWorld(iconRocket);
        int childCount = boardRoot.childCount;
        int value = Random.Range(0, childCount);
        Transform cell1 = boardRoot.GetChild(value);
        Transform cell2 = board.GetComponent<Board>().GetCellId(cell1, cell1.GetComponent<Cell>().GetId());
        SpawmRocket(posSpawn, cell1);
        SpawmRocket(posSpawn, cell2);
    }

    private Vector3 ConvertPosSceneToWorld(GameObject iconUI)
    {
        RectTransform uiRectTransform = iconUI.GetComponent<RectTransform>();
        Vector3 screenPos = uiRectTransform.position;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, distanceFromCamera)
        );
        return worldPos;
    }

    private void SpawmRocket(Vector3 pos, Transform cellTarget)
    {
        GameObject rocket1 = Instantiate(rocketPrefab, pos, Quaternion.identity);
        rocket1.GetComponent<Rocket>().target = cellTarget;
        rocket1.GetComponent<Rocket>().gameUtils = this;
        rocket1.GetComponent<Rocket>().board = board.GetComponent<Board>();
    }

    private void SwapBoardCell()
    {
        List<Transform> cells = GetCells();
        List<Vector2> vectorMatrix = GetListVector2Matrix();

        for(int i = 0; i < vectorMatrix.Count; i++)
        {
            Vector2 vector2 = vectorMatrix[i];
            int row = (int)vector2.x;
            int col = (int)vector2.y;
            int childCount = cells.Count;
            int value = Random.Range(0, childCount);
            Transform cell = cells[value];
            cell.position = board.GetComponent<Board>().GetPositionMatrix(row, col);
            cell.GetComponent<Cell>().SetupRowAndCol(row, col);
            board.GetComponent<Board>().SetIdInMatrix(row, col, cell.GetComponent<Cell>().GetId());
            cells.Remove(cell);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShootRocket();
        }
        else if(Input.GetKeyDown(KeyCode.N))
        {
            SwapBoardCell();
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            FindConnectableCells();
        }
    }

    public void StartUtils(UtilType type)
    {
        switch (type)
        {
            case UtilType.Rocket:
                ShootRocket();
                break;
            case UtilType.Swap:
                SwapBoardCell();
                break;
            case UtilType.Find:
                FindConnectableCells();
                break;
            case UtilType.Time:
                AddTimer();
                break;
            default:
                return;
        }
    }
    private List<Transform> GetCells()
    {
        List<Transform> cells = new List<Transform>();
        for(int i = 0; i < boardRoot.childCount; i++)
        {
            cells.Add(boardRoot.GetChild(i).transform);
        }
        return cells;
    }
    private List<Vector2> GetListVector2Matrix()
    {
        List<Vector2> vector2s = new List<Vector2>();
        int[,] matrix = board.GetComponent<Board>().GetMatrix();
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if(matrix[i, j] != -1)
                {
                    vector2s.Add(new Vector2(i, j));
                }
            }
        }

        return vector2s;
    }

    private Dictionary<int, List<Transform>> GetDicCells()
    {
        Dictionary<int, List<Transform>> cells = new Dictionary<int, List<Transform>>();

        for (int i = 0; i < boardRoot.childCount; i++)
        {
            int id = boardRoot.GetChild(i).GetComponent<Cell>().GetId();

            if (!cells.ContainsKey(id))
            {
                List<Transform> listCell = new List<Transform>();
                listCell.Add(boardRoot.GetChild(i));
                cells.Add(id, listCell);
            }
            else
            {
                List<Transform> listCell = cells[id];
                listCell.Add(boardRoot.GetChild(i));
                cells[id] = listCell;
            }
        }
        return cells;   
    }

    private void FindConnectableCells()
    {
        int[,] matrixPadding = PikachuGameLogic.Instance.GetPaddedMatrix(board.GetComponent<Board>().GetMatrix());
        Dictionary<int, List<Transform>> cells = GetDicCells();
        bool checkConnectCell = false; 
        foreach (KeyValuePair<int, List<Transform>> pair in cells)
        {
            int key = pair.Key;
            List<Transform> list = pair.Value;

            for (int i = 0; i < list.Count - 1; i++)
            {
                if (!checkConnectCell)
                {
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        if (!checkConnectCell)
                        {

                            Vector2Int posA = list[i].GetComponent<Cell>().GetVector2RowAndCol();
                            Vector2Int posB = list[j].GetComponent<Cell>().GetVector2RowAndCol();
                            checkConnectCell = PikachuGameLogic.Instance.GetCanConnect(matrixPadding,
                                posA.x, posA.y, posB.x, posB.y);

                            if (checkConnectCell)
                            {
                                list[i].GetComponent<Cell>().Highlight();
                                list[j].GetComponent<Cell>().Highlight();
                            }
                            return;
                        }
                    }
                }
            }
        }
    }

    private void AddTimer()
    {
        float currentTimer = LevelTimeManager.Instance.Timer;
        LevelTimeManager.Instance.SetTimer(currentTimer + timer);
    }
}
