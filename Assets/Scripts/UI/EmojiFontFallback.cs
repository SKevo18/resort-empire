using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace ResortEmpire.UI
{
    /// <summary>
    /// Registers the operating system's color emoji font as a TextMeshPro emoji fallback, so emoji
    /// typed into TMP text render without shipping an emoji atlas. The font asset is dynamic and
    /// in-memory only; it is never saved into the project, and is rebuilt when an editor domain
    /// reload has unloaded its atlas.
    /// </summary>
    internal static class EmojiFontFallback
    {
        private const string FontAssetName = "OS Color Emoji (runtime)";

        // Apple Color Emoji (macOS/iOS), NotoColorEmoji (Android/Linux), seguiemj (Windows).
        private static readonly string[] EmojiFontFileHints = { "emoji", "seguiemj" };

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            var fallbacks = TMP_Settings.emojiFallbackTextAssets;
            if (fallbacks == null) return; // TMP Essentials not imported.

            var existing = fallbacks.Find(asset => asset != null && asset.name == FontAssetName) as TMP_FontAsset;
            if (existing != null && IsIntact(existing)) return;

            // Stale entry: atlas unloaded by a domain reload, or nulled by saving TMP Settings.
            if (existing != null) UnityEngine.Object.DestroyImmediate(existing);
            fallbacks.RemoveAll(asset => asset == null || asset.name == FontAssetName);

            string path = Array.Find(Font.GetPathsToOSFonts(), IsEmojiFontFile);
            if (path == null)
            {
                Debug.LogWarning("No OS emoji font found; emoji in TMP text will render as missing glyphs.");
                return;
            }

            var fontAsset = TMP_FontAsset.CreateFontAsset(path, 0, 64, 4, GlyphRenderMode.COLOR, 512, 512);
            if (fontAsset == null) return;

            fontAsset.name = FontAssetName;
            fontAsset.hideFlags = HideFlags.DontSave;
            fallbacks.Add(fontAsset);
        }

        private static bool IsEmojiFontFile(string path)
        {
            string file = Path.GetFileName(path);
            return Array.Exists(EmojiFontFileHints, hint => file.IndexOf(hint, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static bool IsIntact(TMP_FontAsset fontAsset) =>
            fontAsset.material != null && Array.TrueForAll(fontAsset.atlasTextures, texture => texture != null);
    }
}
