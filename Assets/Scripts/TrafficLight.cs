using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public float redTime = 5f;
    public float yellowTime = 2f;
    public float greenTime = 5f;

    private Light redLight;
    private Light yellowLight;
    private Light greenLight;

    public bool redOn = false;
    public bool yellowOn = false;
    public bool greenOn = false;

    void Start()
    {
        //Obtener referencias a los componentes Light de cada luz
        redLight = transform.Find("luz roja").GetComponent<Light>();
        yellowLight = transform.Find("luz amarilla").GetComponent<Light>();
        greenLight = transform.Find("luz verde").GetComponent<Light>();

        //Iniciar el semáforo
        redLight.intensity = 0;
        yellowLight.intensity = 0;
        greenLight.intensity = 0;
        StartCoroutine("ChangeLights");
    }

    IEnumerator ChangeLights()
    {
        while (true)
        {
            //Encender luz roja
            yellowLight.intensity = 0; yellowOn = false;
            redLight.intensity = 20; redOn = true;
            yield return new WaitForSeconds(redTime);

            //Encender luz verde
            redLight.intensity = 0; redOn = false;
            greenLight.intensity = 20; greenOn = true;
            yield return new WaitForSeconds(greenTime);

            //Encender luz amarilla
            greenLight.intensity = 0; greenOn = false;
            yellowLight.intensity = 20; yellowOn = true;
            yield return new WaitForSeconds(yellowTime);
        }
    }
}
