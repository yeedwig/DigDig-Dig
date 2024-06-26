using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    private Light2D pointLight;
    private bool isFlickering = false;
    private float timeDelay;
    private float intensity;
    [SerializeField] private float maxDelay;
    [SerializeField] private float minDelay;

    public float maxIntensity;
    public float minIntensity;

    // Start is called before the first frame update
    void Start()
    {
        pointLight = GetComponent<Light2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }



        //Debug.Log(pointLight.intensity);
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        timeDelay = Random.Range(minDelay, maxDelay);
        intensity = Random.Range(minIntensity, maxIntensity);
        pointLight.intensity = intensity;
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }

}
