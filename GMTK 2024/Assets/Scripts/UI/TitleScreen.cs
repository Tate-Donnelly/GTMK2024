using UnityEngine;

namespace UI
{
    /// <summary>
    /// Handles title screen
    /// </summary>
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField] private GameObject SettingsView;

        public void Play()
        {
            
        }
        
        /// <summary>
        /// Toggle on and off the settings view
        /// </summary>
        public void ToggleSettingsView()
        {
            SettingsView.SetActive(!SettingsView.activeSelf);
        }

        /// <summary>
        /// Quit Game
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
