using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private GameObject mainMenuPanel;

    void OnEnable()
    {
        volumeSlider.value = SaveManager.Instance.CurrentData.musicVolume;
    }

    public void OnVolumeChanged(float value)
    {
        SaveManager.Instance.CurrentData.musicVolume = value;
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnBackPressed()
    {
        SaveManager.Instance.Save();
        gameObject.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
