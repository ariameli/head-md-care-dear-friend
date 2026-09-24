using UnityEngine;
using Yarn.Unity;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light targetLight;

    private void Awake()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }
    }

    [YarnCommand("setLightTemperature")]
    public void SetLightTemperature(float temperature)
    {
        if (targetLight == null)
        {
            Debug.LogWarning("Aucune lumière assignée à LightManager.");
            return;
        }

        targetLight.useColorTemperature = true;
        targetLight.colorTemperature = Mathf.Clamp(temperature, 1000f, 20000f);
    }
}