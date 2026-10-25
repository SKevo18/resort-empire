using UnityEngine;

namespace ResortEmpire.Audio
{
    /// <summary>
    /// Plays short 2D UI sound effects through one shared <see cref="AudioSource"/> that
    /// survives scene loads. Uses the player's <see cref="VolumeSettings.SoundVolume"/>.
    /// </summary>
    public static class UiSound
    {
        private static AudioSource _source;

        /// <summary>Plays <paramref name="clip"/> once; overlapping calls mix instead of cutting off.</summary>
        public static void Play(AudioClip clip, float volumeScale = 1f)
        {
            if (clip == null) return;

            if (_source == null)
            {
                var go = new GameObject("UI Sound");
                Object.DontDestroyOnLoad(go);
                _source = go.AddComponent<AudioSource>();
                _source.playOnAwake = false;
                _source.spatialBlend = 0f;
            }

            _source.PlayOneShot(clip, VolumeSettings.SoundVolume * volumeScale);
        }
    }
}
