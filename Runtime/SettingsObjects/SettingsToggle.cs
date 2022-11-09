using UnityEngine;
using Pixygon.Tooltip;

namespace Pixygon.Settings {
    public class SettingsToggle : SettingsObject {
        public override void Activate(SettingsObjectData data) {
            base.Activate(data);
            var value = SettingsSubsystem.instance.GetBoolValue(_data.valueName);
            _settingsValue.text = value ? "On" : "Off";
            GetComponent<TooltipTrigger>()?.SetTooltip(data.valueName , string.Empty , data.description);
        }
        public void UpdateValue(bool value) {
            _settingsValue.text = value ? "On" : "Off";
            SettingsSubsystem.instance.SetBoolValue(_data.valueName, value);    
            GetComponent<AudioSource>().Play(); 
        }
    }
}