using System;
using UnityEngine;

namespace Pixygon.Settings {
    [Serializable, CreateAssetMenu(fileName = "SettingsIntData", menuName = "Pixygon/Settings/Int Value")]
    public class SettingsIntData : SettingsObjectData {
        public int minValue;
        public int maxValue;
        public bool UseLabels;
        public string[] Labels;
    }
}