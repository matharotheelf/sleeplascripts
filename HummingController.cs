using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class HummingController : MonoBehaviour
{
    [SerializeField] AudioPitchEstimator audioPitchEstimator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Material skybox;

    [SerializeField] AudioSource birdsAudioSource;
    [SerializeField] AudioSource cricketsAudioSource;

    [SerializeField] GameObject starSystem;

    [SerializeField] float frequencyLow;
    [SerializeField] float frequencyHigh;
    [SerializeField] float environmentColourParameterLow = 0;
    [SerializeField] float environmentColourParameterHigh = 255;
    public float environmentColourParameter = 0.25f;
    [SerializeField] float smoothCoef;

    [SerializeField] Color daySkyColor;
    [SerializeField] Color nightSkyColor;

    [SerializeField] Color dayMaterialsColor;
    [SerializeField] Color nightMaterialsColor;

    [SerializeField] Material[] forestMaterials;

    [SerializeField] ParticleSystem fireflies;
    [SerializeField] int fireflyMaxCount = 3000;
    [SerializeField] Light sun;




    private Color skyColor;
    private Color materialsColor;

    // Start is called before the first frame update
    void Start()
    {
        connectMicrophone();

        // set initial sky colour to neutral
        skyColor = new Color(environmentColourParameter, environmentColourParameter, environmentColourParameter);
        RenderSettings.skybox.SetColor("_Tint", skyColor);

        // run the pitch estimation 10 times a second which updates the environment
        InvokeRepeating("EstimatePitch", 0, 0.1f);
    }

    private void connectMicrophone()
    {
        Debug.Log(Microphone.devices);
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 1800, 44100);
        audioSource.Play();
    }

    // run to estimate the pitch and the environment responds
    void EstimatePitch()
    {
        // Estimates fundamental frequency from AudioSource output.
        float frequency = audioPitchEstimator.Estimate(audioSource);

        if (float.IsNaN(frequency))
        {
            Debug.Log("Frequency Not Found");
        }
        else
        {

            // map the frequency within the frequency range to within the environment parameters range
            // environment parameter controls all the variables which react in the environment
            float normal = Mathf.InverseLerp(frequencyLow, frequencyHigh, frequency);
            float newEnvironmentColourParameter = Mathf.Lerp(environmentColourParameterHigh, environmentColourParameterLow, normal);

            // smooth the environment parameter
            environmentColourParameter = environmentColourParameter + smoothCoef * (newEnvironmentColourParameter - environmentColourParameter);

            // update lighting conditions
            updateLightingConditions(environmentColourParameter);

            // darken materials surrounding
            darkenSurroundings(environmentColourParameter);

            // create fireflies surrounding
            createFireflies(environmentColourParameter);

            // play responsive sounds crickets and birds
            varyEnvironmentSounds(environmentColourParameter);

            // hide or reveal sphere of stars
            hideOrRevealStars(environmentColourParameter);
        }
    }

    void darkenSurroundings(float parameter)
    {
        materialsColor = LerpHSV(nightMaterialsColor, dayMaterialsColor, parameter);

        foreach (Material material in forestMaterials)
        {
            material.SetColor("_Color", materialsColor);
        }
    }

    void varyEnvironmentSounds(float parameter)
    {
        birdsAudioSource.volume = 2f * (parameter - 0.5f);

        cricketsAudioSource.volume = 2f * (0.5f - parameter);
    }

    void hideOrRevealStars(float parameter)
    {
        if (parameter < 0.3f)
        {
            starSystem.SetActive(true);
        }
        else
        {
            starSystem.SetActive(false);
        }
    }

    void createFireflies(float parameter)
    {
        var emission = fireflies.emission;
        emission.rateOverTime = fireflyMaxCount * (0.5f - parameter);
    }

    void updateLightingConditions(float parameter)
    {
        skyColor = LerpHSV(nightSkyColor, daySkyColor, environmentColourParameter);

        skybox.SetColor("_Tint", skyColor);

        skybox.SetFloat("_AtmosphereThickness", environmentColourParameter + 0.1f);

        sun.intensity = environmentColourParameter;
    }

    private static Color LerpHSV(Color initial, Color final, float t)
    {
        float initial_h;
        float initial_s;
        float initial_v;
        float final_h;
        float final_s;
        float final_v;

        // Convert colours to HSV
        Color.RGBToHSV(initial, out initial_h, out initial_s, out initial_v);
        Color.RGBToHSV(final, out final_h, out final_s, out final_v);

        // lerp values and convert back to RGB
        return Color.HSVToRGB
        (
            initial_h + t * (final_h - initial_h),    // H
            initial_s + t * (final_s - initial_s),    // S
            initial_v + t * (final_v - initial_v)    // V
        );
    }
}
