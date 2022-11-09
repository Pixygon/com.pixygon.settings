using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Pixygon.Core;

namespace Pixygon.Settings {
    public class SettingsSubsystem : SubsystemModule {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private SettingsObjectData[] _gameplaySettings;
        [SerializeField] private SettingsObjectData[] _graphicsSettings;
        [SerializeField] private SettingsObjectData[] _audioSettings;
        public Resolution[] supportedResolutions { get; private set; }
        [SerializeField] private UniversalRenderPipelineAsset urpAsset;
        [SerializeField] private VolumeProfile postProcVolume;
        [SerializeField] private VolumeProfile postProcVolumeDefault;
        [SerializeField] private VolumeProfile skyVolume;
        public VolumeProfile PostProc => postProcVolume;
        public VolumeProfile SkyVolume => skyVolume;
        public SettingsObjectData[] GameplaySettings => _gameplaySettings;
        public SettingsObjectData[] GraphicsSettings => _graphicsSettings;
        public SettingsObjectData[] AudioSettings => _audioSettings;
        public SettingsData SettingsData { get; private set; }

        public static SettingsSubsystem instance;

        public static event Action OnGraphicsSettingsUpdated;
        
        
        private void Start() {
            if (instance != null)
                Destroy(this);
            else
                Initialize();
        }

        protected override void Initialize() {
            instance = this;
            supportedResolutions = GetSupportedResolutions();
            if (!PlayerPrefs.HasKey("SettingsData")) {
                SettingsData = new SettingsData();
                SettingsData.Init(postProcVolumeDefault);
                SaveSettings();
            }

            SettingsData = JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString("SettingsData"));
            UpdateActorSubsystem();
            UpdateAudioSettings();
            UpdateGraphicsSettings();
            PlayerPrefs.Save();
        }

        private Resolution[] GetSupportedResolutions() {
            return Screen.resolutions.OrderByDescending(res => res.height * res.width).ToArray();
        }

        public void ApplySettings() {
            SaveSettings();
            UpdateActorSubsystem();
            UpdateAudioSettings();
            UpdateGraphicsSettings();
            UpdateNetworkpanels();
        }

        public void RestoreDefaultSettings() {
            Debug.Log("Restore default settings...");
            SettingsData = new SettingsData();
            SettingsData.Init(postProcVolumeDefault);
            SaveSettings();
            UpdateActorSubsystem();
            UpdateAudioSettings();
            UpdateGraphicsSettings();
            UpdateNetworkpanels();
        }

        public int GetGraphicsLevel {
            get { return SettingsData.GraphicsLevel; }
        }

        public void SetGraphicsLevel(int i) {
            SettingsData.GraphicsLevel = i;
            switch (i) {
                case 0:
                    //Very Low
                    SettingsData.QualityLevel = 0;
                    SettingsData.TextureQuality = 0;
                    SettingsData.VolumetricFogResolution = 0;
                    SettingsData.VolumetricCloudQuality = 0;
                    SettingsData.DrawGrass = 0;

                    SettingsData.AmbientOcclusion = 0;
                    SettingsData.Bloom = 0;
                    SettingsData.ContactShadows = 0;
                    SettingsData.DrawDistance = 500;
                    SettingsData.MicroShadows = false;
                    SettingsData.MotionBlur = 0;
                    SettingsData.SSReflections = 0;
                    SettingsData.SubsurfaceQuality = 0;
                    SettingsData.FPSCap = 0;
                    SettingsData.DistantShadowResolution = 0;
                    SettingsData.DepthofField = false;
                    break;
                case 1:
                    //Low
                    SettingsData.QualityLevel = 1;
                    SettingsData.TextureQuality = 1;
                    SettingsData.VolumetricFogResolution = 0;
                    SettingsData.VolumetricCloudQuality = 1;
                    SettingsData.DrawGrass = 0;

                    SettingsData.AmbientOcclusion = 1;
                    SettingsData.Bloom = 1;
                    SettingsData.ContactShadows = 1;
                    SettingsData.DrawDistance = 1500;
                    SettingsData.MicroShadows = false;
                    SettingsData.MotionBlur = 1;
                    SettingsData.SSReflections = 0;
                    SettingsData.SubsurfaceQuality = 0;
                    SettingsData.LocalShadowQuality = 0;
                    SettingsData.FPSCap = 1;
                    SettingsData.DistantShadowResolution = 0;
                    SettingsData.DepthofField = false;
                    break;
                case 2:
                    //Medium
                    SettingsData.QualityLevel = 1;
                    SettingsData.TextureQuality = 2;
                    SettingsData.VolumetricFogResolution = 1;
                    SettingsData.VolumetricCloudQuality = 2;
                    SettingsData.DrawGrass = 1;

                    SettingsData.AmbientOcclusion = 1;
                    SettingsData.Bloom = 2;
                    SettingsData.ContactShadows = 2;
                    SettingsData.DrawDistance = 3000;
                    SettingsData.MicroShadows = true;
                    SettingsData.MotionBlur = 1;
                    SettingsData.SSReflections = 1;
                    SettingsData.SubsurfaceQuality = 1;
                    SettingsData.LocalShadowQuality = 1;
                    SettingsData.FPSCap = 1;
                    SettingsData.DistantShadowResolution = 0;
                    SettingsData.DepthofField = false;
                    break;
                case 3:
                    //High
                    SettingsData.QualityLevel = 2;
                    SettingsData.TextureQuality = 3;
                    SettingsData.VolumetricFogResolution = 2;
                    SettingsData.VolumetricCloudQuality = 3;
                    SettingsData.DrawGrass = 2;

                    SettingsData.AmbientOcclusion = 2;
                    SettingsData.Bloom = 3;
                    SettingsData.ContactShadows = 3;
                    SettingsData.DrawDistance = 5000;
                    SettingsData.MicroShadows = true;
                    SettingsData.MotionBlur = 2;
                    SettingsData.SSReflections = 3;
                    SettingsData.SubsurfaceQuality = 3;
                    SettingsData.LocalShadowQuality = 2;
                    SettingsData.FPSCap = 2;
                    SettingsData.DistantShadowResolution = 1;
                    SettingsData.DepthofField = true;
                    break;
                case 4:
                    //Ultra
                    SettingsData.QualityLevel = 2;
                    SettingsData.TextureQuality = 3;
                    SettingsData.VolumetricFogResolution = 3;
                    SettingsData.VolumetricCloudQuality = 4;
                    SettingsData.DrawGrass = 3;
                    SettingsData.LocalShadowQuality = 2;
                    SettingsData.AmbientOcclusion = 3;
                    SettingsData.Bloom = 3;
                    SettingsData.ContactShadows = 3;
                    SettingsData.DrawDistance = 10000;
                    SettingsData.MicroShadows = true;
                    SettingsData.MotionBlur = 2;
                    SettingsData.SSReflections = 3;
                    SettingsData.SubsurfaceQuality = 3;
                    SettingsData.LocalShadowQuality = 3;
                    SettingsData.FPSCap = 3;
                    SettingsData.DistantShadowResolution = 2;
                    SettingsData.DepthofField = true;
                    break;
            }

            SaveSettings();
            UpdateAudioSettings();
            UpdateActorSubsystem();
            UpdateGraphicsSettings();
        }

        private void UpdateAudioSettings() {
            mixer.SetFloat("MasterVolume", Mathf.Log10(SettingsData.MasterVolume) * 20f);
            mixer.SetFloat("VoiceVolume", Mathf.Log10(SettingsData.VoiceVolume) * 20f);
            mixer.SetFloat("AmbienceVolume", Mathf.Log10(SettingsData.AmbienceVolume) * 20f);
            mixer.SetFloat("SFXVolume", Mathf.Log10(SettingsData.SfxVolume) * 20f);
            mixer.SetFloat("NFTVolume", Mathf.Log10(SettingsData.NFTVolume) * 20f);
        }

        private void UpdateGraphicsSettings() {
            QualitySettings.SetQualityLevel(SettingsData.QualityLevel, true);
            switch (SettingsData.TextureQuality) {
                case 0:
                    QualitySettings.masterTextureLimit = 3;
                    break;
                case 1:
                    QualitySettings.masterTextureLimit = 2;
                    break;
                case 2:
                    QualitySettings.masterTextureLimit = 1;
                    break;
                case 3:
                    QualitySettings.masterTextureLimit = 0;
                    break;
            }
            //Application.targetFrameRate = 60;
            switch (SettingsData.FPSCap) {
                case 0:
                    Application.targetFrameRate = 24;
                    break;
                case 1:
                    Application.targetFrameRate = 30;
                    break;
                case 2:
                    Application.targetFrameRate = 45;
                    break;
                case 3:
                    Application.targetFrameRate = 60;
                    break;
                case 4:
                    Application.targetFrameRate = 90;
                    break;
                case 5:
                    Application.targetFrameRate = 120;
                    break;
                case 6:
                    Application.targetFrameRate = 240;
                    break;
            }

            switch (SettingsData.AnisotropicFiltering) {
                case 0:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 2:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
            }

            QualitySettings.vSyncCount = SettingsData.VSync;

            if (Camera.main != null)
                Camera.main.farClipPlane = SettingsData.DrawDistance;

            if (string.IsNullOrEmpty(SettingsData.ScreenshotPath)) {
                SettingsData.ScreenshotPath = Application.persistentDataPath;
            }

            if (SettingsData.screenResolution < supportedResolutions.Length && SettingsData.screenResolution >= 0) {
                var res = supportedResolutions[SettingsData.screenResolution];
                var currentRes = Screen.currentResolution;
                if (currentRes.width != res.width || currentRes.height != res.height || currentRes.refreshRate != res.refreshRate) {
                    Screen.SetResolution(res.width, res.height, SettingsData.Fullscreen, res.refreshRate);
                }
            }

            OnGraphicsSettingsUpdated?.Invoke();
            UpdatePostProcessing();
            UpdateSkySettings();
        }

        private void UpdatePostProcessing() {
            /*
            if(postProcVolume.TryGet(out AmbientOcclusion ao)) {
                ao.active = SettingsData.AmbientOcclusion != 0 ? true : false;
                ao.quality.levelAndOverride = SettingsData.AmbientOcclusion != 0 ? (SettingsData.AmbientOcclusion - 1, false) : (0, false);
                ao.intensity.value = SettingsData.AmbientOcclusion != 0 ? SettingsData.AmbientOcclusionIntensity : 0f;
            }
            */


            if(postProcVolume.TryGet(out MotionBlur blur)) {
                blur.active = false;
                //blur.active = SettingsData.MotionBlur != 0 ? true : false;
                //blur.quality.levelAndOverride = SettingsData.MotionBlur != 0 ? (SettingsData.MotionBlur - 1, false) : (0, false);
                blur.intensity.value = SettingsData.MotionBlur != 0 ? SettingsData.MotionBlurIntensity : 0f;
            }

            if (postProcVolume.TryGet(out ColorAdjustments color)) {
                color.postExposure.value = Mathf.Lerp(-5f, 5f, SettingsData.ScreenBrightness * .01f);
            }

            if (postProcVolume.TryGet(out ChromaticAberration ca)) {
                ca.active = SettingsData.ChromaticAbberation;
            }

            if (postProcVolume.TryGet(out DepthOfField dof)) {
                dof.active = SettingsData.DepthofField;
            }

            if (postProcVolume.TryGet(out FilmGrain filmGrain)) {
                filmGrain.active = SettingsData.FilmGrain;
                filmGrain.intensity.value = SettingsData.FilmGrain ? SettingsData.FilmGrainIntensity : 0f;
            }

            if(postProcVolume.TryGet(out Bloom bloom)) {
                bloom.active = SettingsData.Bloom != 0 ? true : false;
                //bloom.quality.levelAndOverride = SettingsData.Bloom != 0 ? (SettingsData.Bloom - 1, false) : (0, false);
                bloom.intensity.value = SettingsData.Bloom != 0 ? SettingsData.BloomIntensity : 0f;
            }

            /*
            if(postProcVolume.TryGet(out ContactShadows contactShadows)) {
                contactShadows.active = SettingsData.ContactShadows != 0 ? true : false;
                contactShadows.quality.levelAndOverride = SettingsData.ContactShadows != 0 ? (SettingsData.ContactShadows - 1, false) : (0, false);
            }

            if (postProcVolume.TryGet(out MicroShadowing microShadowing)) {
                microShadowing.active = SettingsData.MicroShadows;
            }

            if (postProcVolume.TryGet(out ScreenSpaceReflection ssReflections)) {
                ssReflections.active = SettingsData.SSReflections != 0;
                ssReflections.enabled.overrideState = SettingsData.SSReflections != 0;
                ssReflections.enabledTransparent.overrideState = SettingsData.SSReflections != 0;
                ssReflections.reflectSky.overrideState = SettingsData.SSReflections != 0;
                ssReflections.quality.levelAndOverride = SettingsData.SSReflections != 0 ? (SettingsData.SSReflections - 1, false) : (0, false);
            }

            if (postProcVolume.TryGet(out HDShadowSettings shadows)) {
                switch (SettingsData.LocalShadowQuality) {
                    case 0:
                        shadows.maxShadowDistance.value = 1;
                        break;
                    case 1:
                        shadows.maxShadowDistance.value = 100;
                        break;
                    case 2:
                        shadows.maxShadowDistance.value = 250;
                        break;
                    case 3:
                        shadows.maxShadowDistance.value = 500;
                        break;
                }
            }

            if (postProcVolume.TryGet(out Exposure exposure)) {
                exposure.adaptationSpeedDarkToLight.value = SettingsData.EyeAdaptation;
                exposure.adaptationSpeedLightToDark.value = SettingsData.EyeAdaptation;
            }
            */

            FindObjectOfType<PostProcAssigner>()?.UpdateVolume();
        }

        private void UpdateSkySettings() {
            /*
            if (skyVolume.TryGet(out Fog fog)) {
                if (SettingsData.VolumetricFogResolution == 0)
                    fog.active = false;
                else {
                    fog.active = true;
                    fog.quality.levelAndOverride = (SettingsData.VolumetricFogResolution - 1, false);
                }
            }

            if (skyVolume.TryGet(out VolumetricClouds clouds)) {
                switch (SettingsData.VolumetricCloudQuality) {
                    case 0:
                        clouds.active = false;
                        break;
                    case 1:
                        clouds.active = true;
                        //clouds.cloudMapResolution.value = VolumetricClouds.CloudMapResolution.Low32x32;
                        break;
                    case 2:
                        clouds.active = true;
                        //clouds.cloudMapResolution.value = VolumetricClouds.CloudMapResolution.Medium64x64;
                        break;
                    case 3:
                        clouds.active = true;
                        //clouds.cloudMapResolution.value = VolumetricClouds.CloudMapResolution.High128x128;
                        break;
                    case 4:
                        clouds.active = true;
                        //clouds.cloudMapResolution.value = VolumetricClouds.CloudMapResolution.Ultra256x256;
                        break;
                    case 5:
                        clouds.active = true;
                        //clouds.cloudMapResolution.value = VolumetricClouds.CloudMapResolution.Ultra256x256;
                        break;
                }


                switch (SettingsData.DistantShadowResolution) {
                    case 0:
                        clouds.shadows.value = false;
                        break;
                    case 1:
                        //clouds.shadowResolution.value = VolumetricClouds.CloudShadowResolution.VeryLow64;
                        clouds.shadows.value = true;
                        break;
                    case 2:
                        //clouds.shadowResolution.value = VolumetricClouds.CloudShadowResolution.Low128;
                        clouds.shadows.value = true;
                        break;
                    case 3:
                        //clouds.shadowResolution.value = VolumetricClouds.CloudShadowResolution.Medium256;
                        clouds.shadows.value = true;
                        break;
                    case 4:
                        //clouds.shadowResolution.value = VolumetricClouds.CloudShadowResolution.High512;
                        clouds.shadows.value = true;
                        break;
                    case 5:
                        //clouds.shadowResolution.value = VolumetricClouds.CloudShadowResolution.Ultra1024;
                        clouds.shadows.value = true;
                        break;
                }
            }
            */
            FindObjectOfType<SkyAssigner>()?.UpdateVolume();
        }

        private void UpdateActorSubsystem() {
            Bootstrap.Instance?.ActorSubsystem?.UpdateSettings();
        }

        private void UpdateNetworkpanels() {
            Bootstrap.Instance?.OnlineSubsystem?.UpdateSettings();
        }

        public void SetScreenshotPath(string path) {
            if (string.IsNullOrEmpty(path)) {
                SettingsData.ScreenshotPath = Application.persistentDataPath;
            }
            else {
                SettingsData.ScreenshotPath = path;
            }

            SaveSettings();
        }

        public void SetResolution(Resolutions resolutions) {
            SettingsData.Resolution = resolutions;
            SaveSettings();
            UpdateActorSubsystem();
        }

        private void SaveSettings() {
            PlayerPrefs.SetString("SettingsData", JsonUtility.ToJson(SettingsData));
            PlayerPrefs.Save();
        }


        public int GetIntValue(string valueName) {
            switch (valueName) {
                case "Main Volume":
                    return Mathf.RoundToInt(SettingsData.MasterVolume * 10f);
                case "Voice Volume":
                    return Mathf.RoundToInt(SettingsData.VoiceVolume * 10f);
                case "Ambience Volume":
                    return Mathf.RoundToInt(SettingsData.AmbienceVolume * 10f);
                case "SFX Volume":
                    return Mathf.RoundToInt(SettingsData.SfxVolume * 10f);
                case "NFT Volume":
                    return Mathf.RoundToInt(SettingsData.NFTVolume * 10f);
                case "FOV":
                    return SettingsData.FOV;
                case "Motion Blur Quality":
                    return SettingsData.MotionBlur;
                case "Volumetric Fog Resolution":
                    return SettingsData.VolumetricFogResolution;
                case "Volumetric Cloud Quality":
                    return SettingsData.VolumetricCloudQuality;
                case "Max Dynamic Decals":
                    return SettingsData.Decals;
                case "Screen Resolution":
                    return SettingsData.screenResolution;
                case "Screen Space Reflections Quality":
                    return SettingsData.SSReflections;
                case "Subsurface Scattering Quality":
                    return SettingsData.SubsurfaceQuality;
                case "Ambient Occlusion Quality":
                    return SettingsData.AmbientOcclusion;
                case "Anisotropic Filtering":
                    return SettingsData.AnisotropicFiltering;
                case "Texture Quality":
                    return SettingsData.TextureQuality;
                case "Screen Brightness":
                    return SettingsData.ScreenBrightness;
                case "X Axis Sensitivity":
                    return (int) SettingsData.MouseSensitivityX;
                case "Y Axis Sensitivity":
                    return (int) SettingsData.MouseSensitivityY;
                case "Bloom Quality":
                    return SettingsData.Bloom;
                case "Contact Shadows":
                    return SettingsData.ContactShadows;
                case "Bloom Intensity":
                    return (int) (SettingsData.BloomIntensity * 100f);
                case "Ambient Occlusion Intensity":
                    return (int) (SettingsData.AmbientOcclusionIntensity * 100f);
                case "Motion Blur Intensity":
                    return (int) (SettingsData.MotionBlurIntensity * 100f);
                case "Film Grain Intensity":
                    return (int) (SettingsData.FilmGrainIntensity * 100f);
                case "Local Shadow Quality":
                    return SettingsData.LocalShadowQuality;
                case "VSync":
                    return SettingsData.VSync;
                case "Quality Level":
                    return SettingsData.QualityLevel;
                case "Draw Distance":
                    return SettingsData.DrawDistance;
                case "Draw Grass":
                    return SettingsData.DrawGrass;
                case "FPS Cap":
                    return SettingsData.FPSCap;
                case "Distant Shadow Resolution":
                    return SettingsData.DistantShadowResolution;
                case "Eye Adaptation":
                    return SettingsData.EyeAdaptation;
            }

            return 0;
        }

        public string GetStringValue(string valueName) {
            return "";
        }

        public bool GetBoolValue(string valueName) {
            switch (valueName) {
                case "Fullscreen":
                    return SettingsData.Fullscreen;
                case "Post Processing":
                    return SettingsData.UsePostProcessing;
                case "Show nametags":
                    return SettingsData.ShowNametags;
                case "Notifications":
                    return SettingsData.NotificationSFX;
                case "Chromatic Abberation":
                    return SettingsData.ChromaticAbberation;
                case "Film Grain":
                    return SettingsData.FilmGrain;
                case "Show NSFW":
                    return SettingsData.NSFW;
                case "Push-to-talk":
                    return SettingsData.PushToTalk;
                case "Invert X Axis":
                    return SettingsData.InvertMouseX;
                case "Invert Y Axis":
                    return SettingsData.InvertMouseY;
                case "Button Prompts":
                    return SettingsData.ButtonPrompts;
                case "Micro Shadows":
                    return SettingsData.MicroShadows;
                case "Hide UI":
                    return SettingsData.HideUI;
            }

            return false;
        }

        public void SetIntValue(string valueName, int value) {
            switch (valueName) {
                case "Main Volume":
                    SettingsData.MasterVolume = Mathf.Clamp(value * .1f, 0.0001f, 1f);
                    break;
                case "Voice Volume":
                    SettingsData.VoiceVolume = Mathf.Clamp(value * .1f, 0.0001f, 1f);
                    break;
                case "Ambience Volume":
                    SettingsData.AmbienceVolume = Mathf.Clamp(value * .1f, 0.0001f, 1f);
                    break;
                case "SFX Volume":
                    SettingsData.SfxVolume = Mathf.Clamp(value * .1f, 0.0001f, 1f);
                    break;
                case "NFT Volume":
                    SettingsData.NFTVolume = Mathf.Clamp(value * .1f, 0.0001f, 1f);
                    break;
                case "FOV":
                    SettingsData.FOV = value;
                    break;
                case "Motion Blur Quality":
                    SettingsData.MotionBlur = value;
                    break;
                case "Volumetric Fog Resolution":
                    SettingsData.VolumetricFogResolution = value;
                    break;
                case "Volumetric Cloud Quality":
                    SettingsData.VolumetricCloudQuality = value;
                    break;
                case "Max Dynamic Decals":
                    SettingsData.Decals = value;
                    break;
                case "Screen Resolution":
                    SettingsData.screenResolution = value;
                    break;
                case "Screen Space Reflections Quality":
                    SettingsData.SSReflections = value;
                    break;
                case "Subsurface Scattering Quality":
                    SettingsData.SubsurfaceQuality = value;
                    break;
                case "Ambient Occlusion Quality":
                    SettingsData.AmbientOcclusion = value;
                    break;
                case "Anisotropic Filtering":
                    SettingsData.AnisotropicFiltering = value;
                    break;
                case "Texture Quality":
                    SettingsData.TextureQuality = value;
                    break;
                case "Screen Brightness":
                    SettingsData.ScreenBrightness = value;
                    break;
                case "X Axis Sensitivity":
                    SettingsData.MouseSensitivityX = value;
                    break;
                case "Y Axis Sensitivity":
                    SettingsData.MouseSensitivityY = value;
                    break;
                case "Bloom Quality":
                    SettingsData.Bloom = value;
                    break;
                case "Bloom Intensity":
                    SettingsData.BloomIntensity = Mathf.Clamp(value * .01f, 0f, 1f);
                    break;
                case "Ambient Occlusion Intensity":
                    SettingsData.AmbientOcclusionIntensity = Mathf.Clamp(value * .01f, 0f, 1f);
                    break;
                case "Motion Blur Intensity":
                    SettingsData.MotionBlurIntensity = Mathf.Clamp(value * .01f, 0f, 1f);
                    break;
                case "Film Grain Intensity":
                    SettingsData.FilmGrainIntensity = Mathf.Clamp(value * .01f, 0f, 1f);
                    break;
                case "Contact Shadows":
                    SettingsData.ContactShadows = value;
                    break;
                case "Local Shadow Quality":
                    SettingsData.LocalShadowQuality = value;
                    break;
                case "VSync":
                    SettingsData.VSync = value;
                    break;
                case "Quality Level":
                    SettingsData.QualityLevel = value;
                    break;
                case "Draw Distance":
                    SettingsData.DrawDistance = value;
                    break;
                case "Draw Grass":
                    SettingsData.DrawGrass = value;
                    break;
                case "FPS Cap":
                    SettingsData.FPSCap = value;
                    break;
                case "Distant Shadow Resolution":
                    SettingsData.DistantShadowResolution = value;
                    break;
                case "Eye Adaptation":
                    SettingsData.EyeAdaptation = value;
                    break;
            }
            SaveSettings();
            UpdateAudioSettings();
            UpdateActorSubsystem();
            UpdateGraphicsSettings();
            UpdateNetworkpanels();
        }

        public void SetBoolValue(string valueName, bool value) {
            switch (valueName) {
                case "Fullscreen":
                    SettingsData.Fullscreen = value;
                    break;
                case "Post Processing":
                    SettingsData.UsePostProcessing = value;
                    break;
                case "Show nametags":
                    SettingsData.ShowNametags = value;
                    break;
                case "Chromatic Abberation":
                    SettingsData.ChromaticAbberation = value;
                    break;
                case "Film Grain":
                    SettingsData.FilmGrain = value;
                    break;
                case "Depth of Field":
                    SettingsData.DepthofField = value;
                    break;
                case "Notifications":
                    SettingsData.NotificationSFX = value;
                    break;
                case "Show NSFW":
                    SettingsData.NSFW = value;
                    break;
                case "Push-to-talk":
                    SettingsData.PushToTalk = value;
                    break;
                case "Invert X Axis":
                    Debug.Log("Invert that X-axis!!");
                    SettingsData.InvertMouseX = value;
                    break;
                case "Invert Y Axis":
                    SettingsData.InvertMouseY = value;
                    break;
                case "Button Prompts":
                    SettingsData.ButtonPrompts = value;
                    break;
                case "Micro Shadows":
                    SettingsData.MicroShadows = value;
                    break;
                case "Hide UI":
                    SettingsData.HideUI = value;
                    break;
            }
            SaveSettings();
            UpdateActorSubsystem();
            UpdateGraphicsSettings();
            UpdateNetworkpanels();
        }
    }
}