using System.Collections.Generic;
using UnityEngine;

namespace Pixygon.Settings
{
    public class SettingsPopulator : MonoBehaviour {
        [SerializeField] private GameObject settingsSliderPrefab;
        [SerializeField] private GameObject settingsBoolPrefab;
        [SerializeField] private GameObject settingsHeaderPrefab;
        [SerializeField] private GameObject settingsResolutionPrefab;

        private List<GameObject> settingsObjects;
        public void CleanSettingsData() {
            if(settingsObjects == null) {
                settingsObjects = new List<GameObject>();
                return;
            }
            foreach(GameObject g in settingsObjects) {
                Destroy(g);
            }
            settingsObjects.Clear();
        }

        public void PopulateSettingsData(SettingsObjectData[] data, RectTransform parent) {
            CleanSettingsData();

            foreach(SettingsObjectData s in data) {
                GameObject obj = null;
                if(s is SettingsFloatData || s is SettingsIntData)
                    obj = Instantiate(settingsSliderPrefab, parent);
                else if(s is SettingsBoolData)
                    obj = Instantiate(settingsBoolPrefab, parent);
                else if(s is SettingsHeaderData)
                    obj = Instantiate(settingsHeaderPrefab, parent);
                else if (s is SettingsResolutionData)
                    obj = Instantiate(settingsResolutionPrefab, parent);
                obj.GetComponent<SettingsObject>().Activate(s);
                settingsObjects.Add(obj);
            }
            parent.sizeDelta = new Vector2(0, (settingsObjects.Count * 110f)+300f);
        }
    }
}