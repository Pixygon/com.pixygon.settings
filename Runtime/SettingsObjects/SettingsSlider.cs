using UnityEngine;
using UnityEngine.UI;
using Pixygon.Tooltip;

namespace Pixygon.Settings {
    public class SettingsSlider : SettingsObject {
        [SerializeField] private Slider _slider;

        private bool _isActivating;
        public override void Activate(SettingsObjectData data) {
            _isActivating = true;
            base.Activate(data);
            float value = SettingsSubsystem.instance.GetIntValue(_data.valueName);
            _slider.minValue = ((SettingsIntData)data).minValue;
            _slider.maxValue = ((SettingsIntData)data).maxValue;
            _slider.wholeNumbers = true;// (_data as SettingsIntData).UseLabels;
            _slider.SetValueWithoutNotify(value);
            SetValue();
            GetComponent<TooltipTrigger>()?.SetTooltip(data.valueName , string.Empty , data.description);
            _isActivating = false;
        }
        public void UpdateValue(float value) {
            if(_isActivating)
                return;
            SettingsSubsystem.instance.SetIntValue(_data.valueName, (int)value);
            SetValue();
            GetComponent<AudioSource>().Play();
        }
        private void SetValue() {
            _settingsValue.text = ((SettingsIntData)_data).UseLabels ? ((SettingsIntData)_data)?.Labels[(int)_slider.value] : (_slider.value).ToString();
        }
    }
}