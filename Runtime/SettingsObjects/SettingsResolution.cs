using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Pixygon.Tooltip;
using TMPro;

namespace Pixygon.Settings {
    public class SettingsResolution : SettingsObject {
        [SerializeField] private TMP_Dropdown _select;
        private bool _isActivating;

        public override void Activate(SettingsObjectData data) {
            _isActivating = true;
            base.Activate(data);
            _select.ClearOptions();
            int resIndex = SettingsSubsystem.instance.GetIntValue(_data.valueName);
            int currentIndex = 0;
            List<TMP_Dropdown.OptionData> supportedRes = new List<TMP_Dropdown.OptionData>();

            for (int i = 0; i < SettingsSubsystem.instance.supportedResolutions.Length; i++) {
                Resolution res = SettingsSubsystem.instance.supportedResolutions[i];
                if (resIndex == -1) {
                    if (Screen.currentResolution.width == res.width && Screen.currentResolution.height == res.height)
                        currentIndex = i;
                }
                else {
                    currentIndex = resIndex;
                }
                supportedRes.Add(new TMP_Dropdown.OptionData() {text = res.width + " x " + res.height + " @"+res.refreshRate});
            }

            _select.AddOptions(supportedRes);
            _select.SetValueWithoutNotify(currentIndex);

            GetComponent<TooltipTrigger>()?.SetTooltip(data.valueName, string.Empty, data.description);
            _isActivating = false;
        }

        public void UpdateValue(int value) {
            if (_isActivating)
                return;
            SettingsSubsystem.instance.SetIntValue(_data.valueName, value);

            //_tablet.SliderSFX.Play();
        }
    }
}