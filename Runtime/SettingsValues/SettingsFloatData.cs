using System;
using UnityEngine;

namespace Pixygon.Settings {
    [Serializable, CreateAssetMenu(fileName = "SettingsFloatData", menuName = "Pixygon/Settings/Float Value")]
    public class SettingsFloatData : SettingsObjectData {
        public float defaultValue;
        public float minValue;
        public float maxValue;
    }
}