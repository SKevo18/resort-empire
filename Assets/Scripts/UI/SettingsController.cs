using ResortEmpire.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ResortEmpire.UI
{
    /// <summary>Binds the settings screen's volume sliders to <see cref="VolumeSettings"/>.</summary>
    public sealed class SettingsController : MonoBehaviour
    {
        [SerializeField] private Slider soundVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private string mainMenuScene = "MainMenu";

        private void Awake()
        {
            soundVolumeSlider.SetValueWithoutNotify(VolumeSettings.SoundVolume);
            musicVolumeSlider.SetValueWithoutNotify(VolumeSettings.MusicVolume);
            soundVolumeSlider.onValueChanged.AddListener(value => VolumeSettings.SoundVolume = value);
            musicVolumeSlider.onValueChanged.AddListener(value => VolumeSettings.MusicVolume = value);
        }

        /// <summary>Returns to the main menu scene.</summary>
        public void BackToMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
