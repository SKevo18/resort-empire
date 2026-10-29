using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] private int coins;
        private TextMeshProUGUI coinCountText;

        private void Awake()
        {
            if (coinsSlot == null)
                return;

            var counter = new GameObject("CoinCounter", typeof(RectTransform), typeof(Image));
            var counterRect = (RectTransform)counter.transform;
            counterRect.SetParent(coinsSlot, false);
            counterRect.sizeDelta = new Vector2(180f, 56f);
            var background = counter.GetComponent<Image>();
            background.color = new Color(0.07f, 0.12f, 0.16f, 0.92f);

            var iconObject = new GameObject("CoinIcon", typeof(RectTransform), typeof(Image));
            var iconRect = (RectTransform)iconObject.transform;
            iconRect.SetParent(counterRect, false);
            iconRect.anchorMin = iconRect.anchorMax = new Vector2(0f, 0.5f);
            iconRect.pivot = new Vector2(0f, 0.5f);
            iconRect.anchoredPosition = new Vector2(12f, 0f);
            iconRect.sizeDelta = new Vector2(36f, 36f);
            iconObject.GetComponent<Image>().sprite = CreateCoinSprite();

            var countObject = new GameObject("CoinCount", typeof(RectTransform), typeof(TextMeshProUGUI));
            var countRect = (RectTransform)countObject.transform;
            countRect.SetParent(counterRect, false);
            countRect.anchorMin = new Vector2(0f, 0f);
            countRect.anchorMax = new Vector2(1f, 1f);
            countRect.offsetMin = new Vector2(58f, 0f);
            countRect.offsetMax = new Vector2(-10f, 0f);
            coinCountText = countObject.GetComponent<TextMeshProUGUI>();
            coinCountText.font = TMP_Settings.defaultFontAsset;
            coinCountText.fontSize = 25f;
            coinCountText.fontStyle = FontStyles.Bold;
            coinCountText.color = new Color(1f, 0.96f, 0.83f);
            coinCountText.alignment = TextAlignmentOptions.MidlineLeft;
            SetCoins(coins);
        }

        /// <summary>Updates the number displayed in the coin counter.</summary>
        public void SetCoins(int amount)
        {
            coins = Mathf.Max(0, amount);
            if (coinCountText != null)
                coinCountText.text = coins.ToString("N0");
        }

        private static Sprite CreateCoinSprite()
        {
            const int size = 64;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.name = "GeneratedCoinIcon";
            texture.filterMode = FilterMode.Bilinear;
            var pixels = new Color32[size * size];
            var center = (size - 1) * 0.5f;

            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
            {
                var dx = x - center;
                var dy = y - center;
                var radius = Mathf.Sqrt(dx * dx + dy * dy);
                Color32 color = new Color32(0, 0, 0, 0);
                if (radius <= 29f)
                {
                    color = radius >= 25f
                        ? new Color32(174, 104, 27, 255)
                        : radius >= 22f
                            ? new Color32(255, 218, 112, 255)
                            : new Color32(242, 173, 51, 255);

                    if (x < 25 && y > 35 && radius < 21f)
                        color = new Color32(255, 226, 133, 255);
                    if (Mathf.Abs(dx) < 2f && Mathf.Abs(dy) < 13f && radius < 19f)
                        color = new Color32(194, 119, 31, 255);
                }

                pixels[y * size + x] = color;
            }

            texture.SetPixels32(pixels);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), size);
        }

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
