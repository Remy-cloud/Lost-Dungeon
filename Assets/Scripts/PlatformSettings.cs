using UnityEngine;

public class PlatformSettings : MonoBehaviour
{
    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        Application.targetFrameRate = 30; // lower target for mobile battery/performance
        QualitySettings.SetQualityLevel(1); // lower graphics quality
#elif UNITY_WEBGL
        Application.targetFrameRate = 60;
        QualitySettings.SetQualityLevel(2);
#else
        Application.targetFrameRate = 60; // PC can handle full quality
        QualitySettings.SetQualityLevel(3);
#endif
    }
}
