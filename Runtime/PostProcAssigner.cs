using UnityEngine;
using UnityEngine.Rendering;

namespace Pixygon.Settings {
    public class PostProcAssigner : MonoBehaviour {
        private Volume _volume;
        
        void Start() {
            _volume = GetComponent<Volume>();
            UpdateVolume();
        }

        public void UpdateVolume() {
            if(SettingsSubsystem.instance != null)
                _volume.profile = SettingsSubsystem.instance.PostProc;
        }
    }
}