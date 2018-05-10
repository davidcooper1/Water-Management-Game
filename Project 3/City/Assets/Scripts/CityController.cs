using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour {

    public GameObject Lake;
    public GameObject RainParticles;
    public GameObject SnowParticles;
    public GameObject RainCloud;
    public GameObject Sun;
    public GameObject Lighting;
    public AudioSource RainSound;
    public AudioSource CarSound;
    private float lakeMax;
    private float lakeCurrent;
    private bool lakeUpdateNeeded;
    private Color normal, cold;

    public static readonly int CLEAR = 0;
    public static readonly int RAIN = 1;
    public static readonly int SNOW = 2;

    public MovingCar[] incomingCars;
    public MovingCar[] outgoingCars;

	// Use this for initialization
	void Start () {
        normal = new Color(255, 226, 171, 255);
        cold = new Color(84, 202, 255, 255);
    }
	
	// Update is called once per frame
	void Update () {
        if (lakeUpdateNeeded) {
            Vector3 oldLakePosition = Lake.transform.position;
            Debug.Log("Calc: " + (lakeCurrent / lakeMax) + " Max: " + lakeMax);
            Lake.transform.position = new Vector3(oldLakePosition.x, 6 + 8 * (lakeCurrent / lakeMax), oldLakePosition.z);
            lakeUpdateNeeded = false;
        }
	}

    public void SetLakeMax(float newMax) {
        lakeMax = newMax;
        lakeUpdateNeeded = true;
        Debug.Log("Test: " + lakeMax);
    }

    public void SetLakeCurrent(float newCurrent) {
        lakeCurrent = newCurrent;
        lakeUpdateNeeded = true;
    }

    public void SetWeather(int weather) {
        if (weather == CLEAR) {
            RainParticles.SetActive(false);
            SnowParticles.SetActive(false);
            RainCloud.SetActive(false);
            RainSound.Stop();
        } else if (weather == RAIN) {
            RainParticles.SetActive(true);
            SnowParticles.SetActive(false);
            RainCloud.SetActive(true);
            RainSound.Play();
        } else if (weather == SNOW) {
            RainParticles.SetActive(false);
            SnowParticles.SetActive(true);
            RainCloud.SetActive(true);
            RainSound.Stop();
        }
    }

    public void MigrationIn() {
        foreach (MovingCar car in incomingCars) {
            car.SetMoving(true);
            CarSound.Play();
        }
    }

    public void MigrationOut() {
        foreach (MovingCar car in outgoingCars) {
            car.SetMoving(true);
            CarSound.Play();
        }
    }

    public void MigrationEnd() {
        foreach (MovingCar car in incomingCars) {
            car.SetMoving(false);
        }

        foreach (MovingCar car in outgoingCars) {
            car.SetMoving(false);
        }
        CarSound.Stop();
    }

    public void SetSunOut(bool enabled)
    {
        if (enabled)
        {
            Lighting.GetComponent<Light>().intensity = 3f;
        } else
        {
            Lighting.GetComponent<Light>().intensity = 2f;
        }
        Sun.SetActive(enabled);
    }

    public void SetCoolColor(bool enabled)
    {
        if (enabled)
        {
            Lighting.GetComponent<Light>().color = cold;
        }
        else
        {
            Lighting.GetComponent<Light>().color = normal;
        }
    }

}
