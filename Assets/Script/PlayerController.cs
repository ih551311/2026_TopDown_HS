using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public LevelData levelData;
    public float baseMoveSpeed = 5f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 1.2f;

    [Header("Sprites - 8 Directions")]
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;

    public Sprite[] spriteUpRight;
    public Sprite[] spriteUpLeft;
    public Sprite[] spriteDownRight;
    public Sprite[] spriteDownLeft;

    [Header("Animation")]
    [Range(0.05f, 0.5f)]
    public float frameTime = 0.15f;

    [Header("Engine Sound")]
    public AudioSource engineSound;
    public float minPitch = 0.8f;
    public float maxPitch = 1.6f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;

    private float currentMoveSpeed;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float dashCooldownLeft = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (rb == null || sr == null)
        {
            Debug.LogError("Rigidbody2D 또는 SpriteRenderer가 없습니다!");
            enabled = false;
            return;
        }

        // LevelData에서 속도 가져오기
        if (levelData != null)
            currentMoveSpeed = levelData.playerSpeed;
        else if (PlayerStats.Instance != null)
            currentMoveSpeed = PlayerStats.Instance.GetMoveSpeed();
        else
            currentMoveSpeed = baseMoveSpeed;

        currentSprites = (spriteDown != null && spriteDown.Length > 0) ? spriteDown : null;
        if (currentSprites != null)
            sr.sprite = currentSprites[0];

        if (engineSound != null)
        {
            engineSound.loop = true;
        }
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    private void Update()
    {
        // 대쉬 쿨타임
        if (dashCooldownLeft > 0) dashCooldownLeft -= Time.deltaTime;

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                dashCooldownLeft = dashCooldown;
            }
        }

        // Shift로 대쉬
        if (Keyboard.current.shiftKey.wasPressedThisFrame && !isDashing && dashCooldownLeft <= 0f)
        {
            StartDash();
        }

        float speed = isDashing ? dashSpeed : currentMoveSpeed;
        velocity = input.normalized * speed;

        bool isActuallyMoving = input.sqrMagnitude > 0.01f;

        // 엔진 소리
        if (engineSound != null)
        {
            if (isActuallyMoving)
            {
                if (!engineSound.isPlaying)
                    engineSound.Play();

                float pitch = Mathf.Lerp(minPitch, maxPitch, rb.linearVelocity.magnitude / (dashSpeed * 1.2f));
                engineSound.pitch = pitch;
            }
            else
            {
                if (engineSound.isPlaying)
                {
                    engineSound.pitch = Mathf.Lerp(engineSound.pitch, 0.6f, Time.deltaTime * 3f);
                    if (engineSound.pitch <= 0.65f)
                        engineSound.Stop();
                }
            }
        }

        // 애니메이션
        if (isActuallyMoving)
        {
            timer += Time.deltaTime;
            if (timer >= frameTime)
            {
                timer = 0f;
                frameIndex = (frameIndex + 1) % (currentSprites != null ? currentSprites.Length : 1);
                if (currentSprites != null && currentSprites.Length > 0)
                    sr.sprite = currentSprites[frameIndex];
            }

            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;
            ChangeSprites(GetDirectionSprites(angle));
        }
        else
        {
            frameIndex = 0;
            if (currentSprites != null && currentSprites.Length > 0)
                sr.sprite = currentSprites[0];
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
    }

    // ==================== 벽 충돌 (스테이지별 증가량) ====================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (GameManager.Instance != null && GameTimer.Instance != null)
            {
                float bonus = GameManager.Instance.GetWallBonus();
                GameTimer.Instance.ReduceTime(bonus);   // 벽에 부딪히면 다음 스테이지 시간 증가
            }
        }
    }

    private Sprite[] GetDirectionSprites(float angle)
    {
        if (angle >= 337.5f || angle < 22.5f) return spriteRight;
        else if (angle >= 22.5f && angle < 67.5f) return spriteUpRight;
        else if (angle >= 67.5f && angle < 112.5f) return spriteUp;
        else if (angle >= 112.5f && angle < 157.5f) return spriteUpLeft;
        else if (angle >= 157.5f && angle < 202.5f) return spriteLeft;
        else if (angle >= 202.5f && angle < 247.5f) return spriteDownLeft;
        else if (angle >= 247.5f && angle < 292.5f) return spriteDown;
        else if (angle >= 292.5f && angle < 337.5f) return spriteDownRight;

        return spriteDown;
    }

    private void ChangeSprites(Sprite[] newSprites)
    {
        if (newSprites == null || newSprites.Length == 0) return;
        if (currentSprites == newSprites) return;

        currentSprites = newSprites;
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentSprites[0];
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}