using UnityEngine;
using UnityEngine.UI;

public class LightingManager : MonoBehaviour
{
    public Light DirectionalLight;
    public LightingPreset Preset;
    [SerializeField, Range(0, 180)] public float TimeOfDay;
    public Slider timeSlider;

    private void Start()
    {
        timeSlider.minValue = 0f;
        timeSlider.maxValue = 180f;
        timeSlider.value = TimeOfDay;
        timeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDestroy()
    {
        timeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        SetTimeOfDay(value);
    }

    public void SetTimeOfDay(float value)
    {
        TimeOfDay = value%180;
        UpdateLighting(TimeOfDay / 180f);
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);

        Color dayFog = Preset.FogColor.Evaluate(timePercent);
        float nightBlend = Preset.NightFogBlend.Evaluate(timePercent);
        RenderSettings.fogColor = Color.Lerp(dayFog, Preset.NightFogColor, nightBlend);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.intensity = Preset.DirectionalIntensity.Evaluate(timePercent);
            DirectionalLight.transform.localRotation =
                Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
