using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip levelCompleteSound;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void OnEnable()
    {
        Health.OnHealthChanged += PlayHitSound;
        SaveManager.OnLevelCompleted += PlayLevelCompleteSound;
    }

    void OnDisable()
    {
        Health.OnHealthChanged -= PlayHitSound;
        SaveManager.OnLevelCompleted -= PlayLevelCompleteSound;
    }

    private void PlayHitSound(Health health, float current, float max)
    {
        if (hitSound != null) sfxSource.PlayOneShot(hitSound);
    }

    private void PlayLevelCompleteSound(int levelIndex)
    {
        if (levelCompleteSound != null) sfxSource.PlayOneShot(levelCompleteSound);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
}
