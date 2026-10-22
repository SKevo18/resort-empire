using UnityEngine;

namespace ResortEmpire.UI
{
    /// <summary>
    /// Global entry point for toast notifications. The toast canvas is loaded from
    /// <c>Resources/NotificationToast</c> on first use and survives scene loads.
    /// </summary>
    public static class Notifications
    {
        private static NotificationToast _toast;

        /// <summary>Shows a toast that closes via its × button or after its display time.</summary>
        public static void Show(string title, string message)
        {
            if (_toast == null)
            {
                var canvas = Object.Instantiate(Resources.Load<GameObject>("NotificationToast"));
                Object.DontDestroyOnLoad(canvas);
                _toast = canvas.GetComponentInChildren<NotificationToast>();
            }

            _toast.Show(title, message);
        }
    }
}
