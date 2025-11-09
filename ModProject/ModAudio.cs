using Duckov;
using Duckov.Options;
using UnityEngine;

namespace Airborne
{
    public class ModAudio : MonoBehaviour
    {
        private AudioSource m_Audio;

        private AudioManager.Bus MasterBus => AudioManager.GetBus("Master");

        private AudioManager.Bus SFXBus => AudioManager.GetBus("Master/SFX");


        void Awake()
        {
            m_Audio = GetComponent<AudioSource>();
            OptionsManager.OnOptionsChanged -= OptionsManager_OnOptionsChanged;
            OptionsManager.OnOptionsChanged += OptionsManager_OnOptionsChanged;
            UpdateVolume();
        }

        void OnDestroy()
        {
            OptionsManager.OnOptionsChanged -= OptionsManager_OnOptionsChanged;
        }

        void OptionsManager_OnOptionsChanged(string key)
        {
            UpdateVolume();
        }

        void UpdateVolume()
        {
            float masterVolume = MasterBus != null ? MasterBus.Volume : 1f;
            float sfxVolume = SFXBus != null ? SFXBus.Volume : 1f;
            float finalVolume = masterVolume * sfxVolume;

            bool isMasterMute = MasterBus?.Mute ?? false;
            bool isSFXMute = SFXBus?.Mute ?? false;
            bool isMuted = isMasterMute || isSFXMute;
            if (m_Audio != null)
            {
                m_Audio.volume = finalVolume;
                m_Audio.mute = isMuted;
            }
        }

        public void Play()
        {
            if (m_Audio != null)
            {
                m_Audio.Play();
            }
        }

        public void Stop()
        {
            if (m_Audio != null)
            {
                m_Audio.Stop();
            }
        }
    }
}
