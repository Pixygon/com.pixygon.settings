using System;
using UnityEngine;

namespace Pixygon.Settings {
    [Serializable, CreateAssetMenu(fileName = "SettingsBoolData", menuName = "Pixygon/Settings/Bool Value")]
    public class SettingsBoolData : SettingsObjectData {
        public bool defaultValue;
    }
}