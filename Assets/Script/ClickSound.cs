using UnityEngine;
using UnityEngine.InputSystem;

public class ClickSound : MonoBehaviour
{
    [Header("클릭할 때 재생할 소리")]
    public AudioClip clickSound;

    private AudioSource audioSource;

    void Awake()
    {
        // AudioSource 자동 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // 마우스 좌클릭 감지
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }
    }
}