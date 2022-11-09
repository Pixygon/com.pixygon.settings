using TMPro;
using UnityEngine;

namespace Pixygon.Settings {
    public class SettingsObject : MonoBehaviour {
        [SerializeField] protected TextMeshProUGUI _settingsText;
        [SerializeField] protected TextMeshProUGUI _settingsValue;
        protected SettingsObjectData _data;
        public virtual void Activate(SettingsObjectData data) {
            _settingsText.text = data.valueName;
            _data = data;
        }
    }
}