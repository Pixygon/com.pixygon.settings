# Pixygon — Settings

Data-driven settings system: build a settings menu from `ScriptableObject`
descriptors and apply the values (graphics, audio, resolution, sky, post-processing).

## Key types

| Type | What it is |
|---|---|
| **`SettingsSubsystem`** | Owns + applies the active settings. |
| **`SettingsPopulator`** | Builds the menu UI from the data. |
| **`SettingsData` / `SettingsObjectData`** | The settings model. |
| **`SettingsObject` + variants** (`SettingsHeader`, `SettingsToggle`, `SettingsSlider`, `SettingsResolution`) | One row each. |
| **`SettingsValues`** (`SettingsStringData`, `SettingsFloatData`, `SettingsResolutionData`) | Stored values. |
| **`SkyAssigner` / `PostProcAssigner`** | Apply sky / post-processing choices. |

## Dependencies

None declared.

## Usage

Author settings descriptors → `SettingsPopulator` renders the menu → `SettingsSubsystem`
applies + persists.

## Status

`0.5.0`. Shared settings UI for every game.
