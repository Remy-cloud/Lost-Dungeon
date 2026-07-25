using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FadeOutThenIn(System.Action onFadedOut)
    {
        StartCoroutine(FadeRoutine(onFadedOut));
    }

    private IEnumerator FadeRoutine(System.Action onFadedOut)
    {
        yield return StartCoroutine(Fade(0f, 1f)); // fade to black
        onFadedOut?.Invoke(); // teleport happens here, while screen is black
        yield return StartCoroutine(Fade(1f, 0f)); // fade back in
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, to);
    }
}
