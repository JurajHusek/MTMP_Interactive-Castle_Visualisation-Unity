using UnityEngine;

[System.Serializable] // allows this asset to be serialized and edited
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptables/Lighting Preset", order = 1)]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor; // ambient light color over time (day/night cycle)
    public Gradient DirectionalColor; // directional light color over time
    public Gradient FogColor; // fog color over time
    public AnimationCurve DirectionalIntensity; // directional light intensity over time
    public Color NightFogColor;  // fog color used during night
    public AnimationCurve NightFogBlend; // controls how much night fog blends in over time


}