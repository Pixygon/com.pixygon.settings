using System;
using UnityEngine;

namespace Pixygon.Settings
{
    [Serializable]
    public class SettingsObjectData : ScriptableObject {
        public string valueName;
        public string description;
    }
}