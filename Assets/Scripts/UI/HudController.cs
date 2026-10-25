using UnityEngine;

namespace ResortEmpire.UI
{
    /// <summary>
    /// HUD layer of the GameWorld scene. Exposes the empty slots that the individual HUD
    /// widgets (coins, gems, reputation, ...) are placed into. The corner containers use
    /// layout groups, so widgets only need a LayoutElement with their preferred size.
    /// </summary>
    public sealed class HudController : MonoBehaviour
    {
        [Header("Top left")]
        [SerializeField] private RectTransform reputationSlot;
        [SerializeField] private RectTransform resortValueSlot;

        [Header("Top right")]
        [SerializeField] private RectTransform coinsSlot;
        [SerializeField] private RectTransform gemsSlot;
        [SerializeField] private RectTransform displaySlot;

        /// <summary>Reputation widget slot (SCRUM-81).</summary>
        public RectTransform ReputationSlot => reputationSlot;

        /// <summary>Resort value widget slot (SCRUM-82).</summary>
        public RectTransform ResortValueSlot => resortValueSlot;

        /// <summary>Coin counter slot (SCRUM-65).</summary>
        public RectTransform CoinsSlot => coinsSlot;

        /// <summary>Gems widget slot (SCRUM-79).</summary>
        public RectTransform GemsSlot => gemsSlot;

        /// <summary>Top right display slot (SCRUM-66).</summary>
        public RectTransform DisplaySlot => displaySlot;

        /// <summary>Shows or hides the whole HUD, e.g. while a full-screen menu is open.</summary>
        public void SetVisible(bool visible) => gameObject.SetActive(visible);
    }
}
