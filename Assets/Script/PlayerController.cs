using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseMoveSpeed = 5f;     // 기본 속도 (Inspector에서 수정 가능)

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

    [Header("Sound")]
    public AudioSource engineSound;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;
    private bool isMoving = false;

    private float currentMoveSpeed;   // ← PlayerStats에서 가져올 실제 속도

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

        // PlayerStats에서 저장된 속도 불러오기
        if (PlayerStats.Instance != null)
        {
            currentMoveSpeed = PlayerStats.Instance.GetMoveSpeed();
        }
        else
        {
            currentMoveSpeed = baseMoveSpeed;
        }

        currentSprites = (spriteDown != null && spriteDown.Length > 0) ? spriteDown : null;
        if (currentSprites != null)
            sr.sprite = currentSprites[0];
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        velocity = input.normalized * currentMoveSpeed;   // ← currentMoveSpeed 사용
        isMoving = input.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            Sprite[] newSprites = GetDirectionSprites(angle);
            ChangeSprites(newSprites);
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

    private void Update()
    {
        if (Input.anyKey)
        {
            if (engineSound != null && !engineSound.isPlaying)
                engineSound.Play();
        }
        else
        {
            if (engineSound != null && engineSound.isPlaying)
                engineSound.Stop();
        }

        if (!isMoving)
        {
            frameIndex = 0;
            if (currentSprites != null && currentSprites.Length > 0)
                sr.sprite = currentSprites[0];
            return;
        }

        timer += Time.deltaTime;

        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex++;

            if (currentSprites != null && frameIndex >= currentSprites.Length)
                frameIndex = 0;

            if (currentSprites != null && currentSprites.Length > 0)
                sr.sprite = currentSprites[frameIndex];
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
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
}