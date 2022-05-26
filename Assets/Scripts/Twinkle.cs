using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Twinkle : MonoBehaviour
{
    public float totalSeconds;
    public float maxIntensity;
    private float waitTime;
    Light2D myLight;

    //This script handles the fade/twinkle/flicker of the torches.
    
    //Cache reference and begin twinkle coroutine.
    void Start()
    {
        myLight = gameObject.GetComponent<Light2D>();
        waitTime = totalSeconds / 2;
        StartCoroutine(TwinkleLight()); 
    }

    //Adjust intensity up and down within a min/max range.
    IEnumerator TwinkleLight()
    {

        while (true)
        {

            while (myLight.intensity < maxIntensity)
            {

                myLight.intensity += Time.deltaTime / waitTime;
                yield return null;
            }
            
            while (myLight.intensity > 9)
            {

                myLight.intensity -= Time.deltaTime / waitTime;
                yield return null;
            }
            yield return null;
        }
        

    }
}
