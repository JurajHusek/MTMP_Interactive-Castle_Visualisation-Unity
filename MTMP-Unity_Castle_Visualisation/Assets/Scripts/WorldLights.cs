using UnityEngine;
using UnityEngine.UI;

public class LightingManager : MonoBehaviour
{
    public Light DirectionalLight;
    public LightingPreset Preset;  // lighting preset with colors, gradients and other data
    [SerializeField, Range(0, 180)] public float TimeOfDay;   // current time of day (0–180 range) used for sun rotation
    public Slider timeSlider; // ui slider controlling time of day

    private void Start()
    {
        timeSlider.minValue = 0f;
        timeSlider.maxValue = 180f;
        timeSlider.value = TimeOfDay;
        timeSlider.onValueChanged.AddListener(OnSliderValueChanged); // listen for slider value changes
    }

    private void OnDestroy()
    {
        timeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value) // update time of day when slider changes
    {
        SetTimeOfDay(value);
    }

    public void SetTimeOfDay(float value) // wrap time value and update lighting
    {
        TimeOfDay = value%180;
        UpdateLighting(TimeOfDay / 180f);
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent); // update ambient light color
        // blend fog color between day and night
        Color dayFog = Preset.FogColor.Evaluate(timePercent);
        float nightBlend = Preset.NightFogBlend.Evaluate(timePercent);
        RenderSettings.fogColor = Color.Lerp(dayFog, Preset.NightFogColor, nightBlend);

        if (DirectionalLight != null)
        {
            // update directional light color and intensity
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.intensity = Preset.DirectionalIntensity.Evaluate(timePercent);
            // rotate light to simulate sun movement
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }
}
