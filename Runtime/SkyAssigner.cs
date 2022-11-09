using UnityEngine;
using UnityEngine.Rendering;

namespace Pixygon.Settings {
    public class SkyAssigner : MonoBehaviour {
        void Start() {
            UpdateVolume();
        }

        public void UpdateVolume() {
            if(SettingsSubsystem.instance != null)
                GetComponent<Volume>().profile = SettingsSubsystem.instance.SkyVolume;
        }
    }
}