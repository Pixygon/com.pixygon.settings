using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pixygon {
    [System.Serializable]
    public class SettingsData {
        public int GraphicsLevel = 2;

        //Audio
        public float MasterVolume = 1f;
        public float VoiceVolume = .9f;
        public float AmbienceVolume = .6f;
        public float SfxVolume = .8f;
        public float NFTVolume = .8f;

        //Gameplay
        public bool TeleportMove = true;
        public bool IncrementalRotation = true;
        public bool SittingHeight = true;
        public bool ShowNametags = true;
        public string Language = "English";

        //Graphics
        public Resolutions Resolution = Resolutions.FullHD;
        public int screenResolution = -1;
        public bool Fullscreen = true;
        public bool UsePostProcessing = true;

        public string ScreenshotPath = "";

        //Gameplay
        public bool ShowDetailedNFTs = false;
        public float MouseSensitivityX = 70f;
        public float MouseSensitivityY = 70f;

        //Graphics
        public int FOV = 60;

        //Gameplay
        public bool InvertMouseX;
        public bool InvertMouseY;
        public bool PushToTalk;
        public bool NSFW = false;
        public bool ButtonPrompts = true;
        public bool NotificationSFX = true;

        //Graphics
        public int QualityLevel = 1;
        public int AmbientOcclusion = 0;
        public bool ChromaticAbberation = false;
        public int Decals = 0;
        public bool DepthofField = false;
        public int Bloom = 0;
        public int MotionBlur = 0;
        public int SSReflections = 0;
        public int SubsurfaceQuality = 0;
        public int VolumetricCloudQuality = 0;
        public int VolumetricFogResolution = 0;
        public int AnisotropicFiltering = 0;
        public int TextureQuality = 2;
        public int ScreenBrightness = 50;
        public int ContactShadows = 0;
        public bool FilmGrain = false;
        public bool MicroShadows = false;
        public float BloomIntensity = .5f;
        public float AmbientOcclusionIntensity = .5f;
        public float MotionBlurIntensity = .5f;
        public float FilmGrainIntensity = .5f;
        public int VSync = 0;
        public int LocalShadowQuality = 0;
        public int DrawDistance = 1000;
        public int DrawGrass = 0;
        public bool HideUI = false;
        public int FPSCap = 1;
        public int DistantShadowResolution = 0;
        public int EyeAdaptation = 2;

        public void Init(VolumeProfile postProc) {
            /*
            if(postProc.TryGet(out AmbientOcclusion ao)) {
                if(!ao.active) AmbientOcclusion = 0;

                else {
                    AmbientOcclusion = ao.quality.levelAndOverride.level + 1;
                }

                AmbientOcclusionIntensity = ao.intensity.value;
            }


            if(postProc.TryGet(out MotionBlur blur)) {
                if(!blur.active) MotionBlur = 0;

                else {
                    MotionBlur = blur.quality.levelAndOverride.level + 1;
                }

                MotionBlurIntensity = blur.intensity.value;
            }
            */

            if(postProc.TryGet(out ColorAdjustments color)) {
                ScreenBrightness = (int) (Mathf.InverseLerp(-5f, 5f, color.postExposure.value) * 100f);
            }

            if(postProc.TryGet(out ChromaticAberration ca)) {
                ChromaticAbberation = ca.active;
            }

            if(postProc.TryGet(out DepthOfField dof)) {
                DepthofField = dof.active;
            }

            if(postProc.TryGet(out FilmGrain filmGrain)) {
                FilmGrain = filmGrain.active;
                FilmGrainIntensity = filmGrain.intensity.value;
            }

            //if(postProc.TryGet(out Bloom bloom)) {
            //    if(!bloom.active) Bloom = 0;
            //    else {
            //        Bloom = bloom.quality.levelAndOverride.level + 1;
            //    }
            //
            //    BloomIntensity = bloom.intensity.value;
            //}

            //if(postProc.TryGet(out ContactShadows contactShadows)) {
            //    if(!contactShadows.active) ContactShadows = 0;
            //
            //    else {
            //        ContactShadows = contactShadows.quality.levelAndOverride.level + 1;
            //    }
            //}

            //if(postProc.TryGet(out MicroShadowing microShadowing)) {
            //    MicroShadows = microShadowing.active;
            //}

            //if(postProc.TryGet(out ScreenSpaceReflection ssReflections)) {
            //    if(!ssReflections.active) SSReflections = 0;
            //} else {
            //    SSReflections = ssReflections.quality.levelAndOverride.level + 1;
            //}

            //if(postProc.TryGet(out Exposure exposure)) {
            //    EyeAdaptation = (int)exposure.adaptationSpeedDarkToLight.value ;
            //}
        }
    }

    public enum Resolutions {
        Default = 0,
        HD = 1,
        FullHD = 2,
        FullHDUW = 3,
        QuadHD = 4,
        QuadHDUW = 5,
        FourK = 6
    }
}