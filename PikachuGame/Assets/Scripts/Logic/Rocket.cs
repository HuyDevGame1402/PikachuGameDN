using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Speed")]
    public float randomSpeed = 5f;
    public float targetSpeed = 7.5f;

    [Header("Phase Time")]
    public float randomPhaseTime = 0.5f;

    [Header("Angle")]
    public float minRandomAngle = 45f;
    public float maxRandomAngle = 90f;

    private Vector2 moveDirection;
    private float timer;
    private bool homingPhase;

    [Header("Rotate")]
    public float rotateSpeed = 10f;

    public GameUtils gameUtils;
    public Board board;
    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Rocket: Target is NULL");
            Destroy(gameObject);
            return;
        }
        Vector2 toTarget = (target.position - transform.position).normalized;
        float angle = Random.Range(minRandomAngle, maxRandomAngle);
        angle *= Random.value > 0.5f ? 1 : -1;
        moveDirection = Quaternion.Euler(0, 0, angle) * toTarget;

        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayRocket();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!homingPhase)
        {
            transform.position += (Vector3)(moveDirection * randomSpeed * Time.deltaTime);

            if (timer >= randomPhaseTime)
            {
                homingPhase = true;
            }
        }
        else
        {
            Vector2 toTarget = (target.position - transform.position).normalized;
            transform.position += (Vector3)(toTarget * targetSpeed * Time.deltaTime);
            float distance = Vector2.Distance(transform.position, target.position);
            if (distance <= 0.2f)
            {
                gameUtils.rocketCompelteCount++;
                LogicGameCell(gameUtils.rocketCompelteCount);
                Destroy(gameObject);
            }
        }
        RotateRocket();
    }

    private void RotateRocket()
    {
        Vector2 dir = homingPhase
            ? (target.position - transform.position).normalized
            : moveDirection;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    private void LogicGameCell(int countRocCompelte)
    {
        int rA = target.GetComponent<Cell>().GetRow();
        int cA = target.GetComponent<Cell>().GetCol();
        board.SetCellEmpty(rA, cA);
        Vector3 posCellA = target.position;
        Destroy(target.gameObject);
        int idVFX = 0;
        VfxEnum vfx = VfxEnum.vfxCell;
        SpawnVfx(VFXManager.Instance.GetVFX(idVFX, vfx), posCellA, idVFX, vfx);

        if(countRocCompelte == 2)
        {
            GameManager.Instance.SetComboCount();
            GameManager.Instance.AddScoreGame();

            if (board.IsBoardEmpty())
            {
                PikachuGameLogic.WINGAME?.Invoke();
            }
        }
    }
    private void SpawnVfx(GameObject vfx, Vector3 pos, int idVFX, VfxEnum vfxEnum)
    {
        if (IsPrefab(vfx))
        {
            GameObject vfxInGame = Instantiate(vfx, pos, Quaternion.identity);
            ObjectPool.Instance.AddVfxDic(idVFX, vfxInGame,
                ObjectPool.Instance.GetDic(vfxEnum));
        }
        else
        {
            vfx.transform.position = pos;
            vfx.SetActive(true);
        }
    }
    bool IsPrefab(GameObject obj)
    {
        return !obj.scene.IsValid();
    }
}
