//using UnityEngine;
//using DG.Tweening;

//public class UIRocketSmooth : MonoBehaviour
//{
//    public RectTransform target;

//    [Header("Speed")]
//    public float speed = 900f;
//    public float turnSpeed = 8f;

//    [Header("Random Start")]
//    public float randomTime = 0.6f;
//    public float randomAngle = 45f;

//    RectTransform rect;
//    Vector2 velocity;
//    float timer;
//    bool homing;

//    void Awake()
//    {
//        rect = GetComponent<RectTransform>();
//    }

//    void Start()
//    {
//        Launch();
//    }

//    void Launch()
//    {
//        // hướng random ban đầu
//        float angle = Random.Range(-randomAngle, randomAngle);
//        velocity = Quaternion.Euler(0, 0, angle) * Vector2.up * speed;

//        // scale pop dùng DOTween (đúng chỗ)
//        rect.localScale = Vector3.zero;
//        rect.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
//    }

//    void Update()
//    {
//        timer += Time.deltaTime;

//        if (timer >= randomTime)
//            homing = true;

//        if (homing && target != null)
//        {
//            Vector2 dirToTarget =
//                ((Vector2)target.anchoredPosition - rect.anchoredPosition).normalized;

//            Vector2 desiredVelocity = dirToTarget * speed;

//            // bẻ lái mượt (steering)
//            velocity = Vector2.Lerp(
//                velocity,
//                desiredVelocity,
//                Time.deltaTime * turnSpeed
//            );
//        }

//        rect.anchoredPosition += velocity * Time.deltaTime;

//        RotateByVelocity();

//        // kiểm tra tới target thật sự
//        if (homing &&
//            Vector2.Distance(rect.anchoredPosition, target.anchoredPosition) < 25f)
//        {
//            HitTarget();
//        }
//    }

//    void RotateByVelocity()
//    {
//        if (velocity.sqrMagnitude < 0.001f) return;

//        float angle =
//            Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;

//        rect.rotation = Quaternion.Lerp(
//            rect.rotation,
//            Quaternion.Euler(0, 0, angle),
//            Time.deltaTime * 12f
//        );
//    }

//    void HitTarget()
//    {
//        enabled = false;

//        // impact effect bằng DOTween
//        rect.DOScale(0f, 0.2f).SetEase(Ease.InBack)
//            .OnComplete(() => Destroy(gameObject));
//    }
//}

using UnityEngine;
using DG.Tweening;

public class UIRocketSmooth : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;

    [Header("Movement Settings")]
    public float randomFlyDuration = 0.5f;
    public float randomFlyDistance = 150f;
    public float flySpeed = 1500f; // Tăng từ 800 lên 1500
    public float acceleration = 3f; // Tăng từ 1.5 lên 3 - tăng tốc nhanh hơn

    [Header("Rotation Settings")]
    public float rotationSpeed = 1200f; // Tăng từ 720 lên 1200 - quay nhanh hơn nhiều
    public float maxRotationSpeed = 2000f; // Tăng từ 1440 lên 2000
    public float speedWhenRotating = 0.6f; // Tăng từ 0.4 lên 0.6 - vẫn giữ tốc độ khi quay
    public float rotationThreshold = 15f; // Góc chênh lệch để coi là "đang quay"
    public float arcRadius = 80f; // Bán kính đường cung khi quay

    [Header("Smooth Settings")]
    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private RectTransform rectTransform;
    private Vector3 velocity;
    private Vector3 randomDirection;
    private float currentSpeed;
    private float randomPhaseTime;
    private bool isTrackingTarget;

    private Vector3 startRandomPos;
    private Vector3 targetRandomPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        LaunchRocket();
    }

    public void LaunchRocket()
    {
        // Random direction
        float randomAngle = Random.Range(0f, 360f);
        randomDirection = new Vector3(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad),
            Mathf.Sin(randomAngle * Mathf.Deg2Rad),
            0
        ).normalized;

        // Set rotation ban đầu
        float angle = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // Initialize
        startRandomPos = rectTransform.anchoredPosition;
        targetRandomPos = (Vector3)startRandomPos + randomDirection * randomFlyDistance;
        randomPhaseTime = 0f;
        currentSpeed = flySpeed * 0.5f; // Tăng từ 0.3 lên 0.5 - bắt đầu nhanh hơn
        velocity = randomDirection * currentSpeed;
        isTrackingTarget = false;
    }

    void Update()
    {
        if (!isTrackingTarget)
        {
            // Phase 1: Bay random với curve
            randomPhaseTime += Time.deltaTime;
            float t = Mathf.Clamp01(randomPhaseTime / randomFlyDuration);

            // Sử dụng curve để bay mượt
            float curveT = speedCurve.Evaluate(t);
            Vector3 newPos = Vector3.Lerp(startRandomPos, targetRandomPos, curveT);
            rectTransform.anchoredPosition = newPos;

            if (t >= 1f)
            {
                isTrackingTarget = true;
            }
        }
        else
        {
            // Phase 2: Track target với smooth rotation và curved path
            if (target == null) return;

            Vector3 currentPos = rectTransform.position;
            Vector3 toTarget = target.position - currentPos;
            float distanceToTarget = toTarget.magnitude;

            if (distanceToTarget < 5f)
            {
                OnReachTarget();
                return;
            }

            // Tính hướng mong muốn
            Vector3 desiredDirection = toTarget.normalized;

            // Nếu rất gần target thì bay thẳng, không quay nữa
            if (distanceToTarget < 100f)
            {
                velocity = desiredDirection * currentSpeed;
                rectTransform.position += velocity * Time.deltaTime;

                // Rotation mượt
                float finalTargetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
                float finalCurrentAngle = rectTransform.eulerAngles.z;
                float finalRotationSpeed = rotationSpeed * Time.deltaTime / 30f;
                float finalSmoothAngle = Mathf.LerpAngle(finalCurrentAngle, finalTargetAngle, finalRotationSpeed);
                rectTransform.rotation = Quaternion.Euler(0, 0, finalSmoothAngle);
                return;
            }

            // Tính góc chênh lệch giữa hướng hiện tại và hướng mục tiêu
            Vector3 currentDirection = velocity.normalized;
            float angleDifference = Vector3.Angle(currentDirection, desiredDirection);

            // Kiểm tra xem có đang quay nhiều không
            bool isRotatingHard = angleDifference > rotationThreshold;

            // Tính hướng di chuyển mới với đường cung
            Vector3 newDirection;

            if (isRotatingHard && angleDifference > 5f)
            {
                // Quay theo đường cung tròn (arc) thay vì quay tại chỗ
                // Tính điểm trung tâm của đường cung
                Vector3 perpendicular = Vector3.Cross(currentDirection, Vector3.forward).normalized;

                // Xác định hướng quay (trái hay phải) để đi đường ngắn nhất
                float cross = currentDirection.x * desiredDirection.y - currentDirection.y * desiredDirection.x;
                float arcDirection = cross > 0 ? 1f : -1f;

                // Smooth steering với arc motion
                float rotationSpeedMultiplier = 2.5f; // Quay nhanh hơn
                float maxRotationThisFrame = maxRotationSpeed * rotationSpeedMultiplier * Time.deltaTime;

                // Kết hợp rotation với centripetal force
                newDirection = Vector3.RotateTowards(
                    currentDirection,
                    desiredDirection,
                    maxRotationThisFrame * Mathf.Deg2Rad,
                    0f
                );

                // Thêm lực hướng tâm để tạo đường cong
                float arcStrength = Mathf.Clamp01(angleDifference / 90f); // Cong nhiều hơn khi góc lớn
                newDirection = (newDirection + perpendicular * arcDirection * arcStrength * 0.3f).normalized;
            }
            else
            {
                // Góc nhỏ thì đi thẳng
                float maxRotationThisFrame = maxRotationSpeed * Time.deltaTime;
                newDirection = Vector3.RotateTowards(
                    currentDirection,
                    desiredDirection,
                    maxRotationThisFrame * Mathf.Deg2Rad,
                    0f
                );
            }

            // Điều chỉnh tốc độ dựa trên việc đang quay
            float targetSpeed = flySpeed;
            if (isRotatingHard)
            {
                // Giảm tốc độ khi đang quay
                targetSpeed = flySpeed * speedWhenRotating;
            }

            // Tăng/giảm tốc smooth
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime * 3f);
            currentSpeed = Mathf.Min(currentSpeed, flySpeed);

            // Cập nhật velocity - bay thẳng không rung
            velocity = newDirection * currentSpeed;

            // Update position
            rectTransform.position += velocity * Time.deltaTime;

            // Smooth rotation theo hướng bay thực tế
            float targetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
            float currentAngle = rectTransform.eulerAngles.z;

            // Tăng tốc độ rotation lên cao để quay cực nhanh
            float rotationLerpSpeed = rotationSpeed * Time.deltaTime / 30f;
            float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationLerpSpeed);
            rectTransform.rotation = Quaternion.Euler(0, 0, smoothAngle);
        }
    }

    void OnReachTarget()
    {
        Debug.Log("Rocket reached target!");

        // Scale punch effect khi chạm target
        rectTransform.DOScale(1.3f, 0.1f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }

    void OnDestroy()
    {
        DOTween.Kill(rectTransform);
    }

    public void ResetAndLaunch(Transform newTarget = null)
    {
        DOTween.Kill(rectTransform);

        if (newTarget != null)
            target = newTarget;

        rectTransform.localScale = Vector3.one;
        isTrackingTarget = false;
        LaunchRocket();
    }

    // Debug visualization
    void OnDrawGizmos()
    {
        if (rectTransform == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(rectTransform.position, 5f);

        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(rectTransform.position, target.position);
        }
    }
}
