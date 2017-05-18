using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLightsScript : MonoBehaviour {
    public Material IndicatorLightMaterial, CrystalMaterial;
    public GameObject ParticleEffectLights;
    public bool activateLights;
    public float LightSpeed, InitialCrystalValue, EndCrystalValue, LightMaxValue, LightsGrayScale, CrystalGrayScale;
	// Use this for initialization
	void Start () {
        IndicatorLightMaterial = transform.FindChild("CheckpointLights").gameObject.GetComponent<Renderer>().material;
        CrystalMaterial = transform.FindChild("CrystalLight").gameObject.GetComponent<Renderer>().material;
        ParticleEffectLights = transform.FindChild("FireFlys").gameObject;
        ParticleEffectLights.SetActive(false);
        CrystalGrayScale = InitialCrystalValue;
    }
	
	// Update is called once per frame
	void Update () {
        IndicatorLightMaterial.SetColor("_TintColor", new Color(IndicatorLightMaterial.GetColor("_TintColor").r, 
            IndicatorLightMaterial.GetColor("_TintColor").g,
            IndicatorLightMaterial.GetColor("_TintColor").b, 
            LightsGrayScale * 0.1f));

        CrystalMaterial.SetColor("_EmissionColor", new Color(CrystalGrayScale,CrystalGrayScale,CrystalGrayScale,CrystalMaterial.GetColor("_EmissionColor").a));

        if (activateLights)
        {
            LightsGrayScale = Mathf.Lerp(LightsGrayScale, LightMaxValue, LightSpeed * Time.deltaTime);
            CrystalGrayScale = Mathf.Lerp(CrystalGrayScale, EndCrystalValue, LightSpeed * Time.deltaTime);
            ParticleEffectLights.SetActive(true);
        }
        else {
            LightsGrayScale = Mathf.Lerp(LightsGrayScale, 0.0f, LightSpeed * Time.deltaTime);
            CrystalGrayScale = Mathf.Lerp(CrystalGrayScale, InitialCrystalValue, LightSpeed * Time.deltaTime);
            ParticleEffectLights.SetActive(false);
        }
    }
}
