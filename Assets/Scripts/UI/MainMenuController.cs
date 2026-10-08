using UnityEngine;

namespace ResortEmpire.UI
{
    public sealed class MainMenuController : MonoBehaviour
    {
        /// <summary>Quits the player, or stops Play Mode in the Unity Editor.</summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
