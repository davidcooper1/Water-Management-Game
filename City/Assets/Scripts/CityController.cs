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
    private float lakeMax;
    private float lakeCurrent;
    private bool lakeUpdateNeeded;

    public static readonly int CLEAR = 0;
    public static readonly int RAIN = 1;
    public static readonly int SNOW = 2;

    public MovingCar[] incomingCars;
    public MovingCar[] outgoingCars;

	// Use this for initialization
	void Start () {
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
        } else if (weather == RAIN) {
            RainParticles.SetActive(true);
            SnowParticles.SetActive(false);
            RainCloud.SetActive(true);
        } else if (weather == SNOW) {
            RainParticles.SetActive(false);
            SnowParticles.SetActive(true);
            RainCloud.SetActive(true);
        }
    }

    public void MigrationIn() {
        foreach (MovingCar car in incomingCars) {
            car.SetMoving(true);
        }
    }

    public void MigrationOut() {
        foreach (MovingCar car in outgoingCars) {
            car.SetMoving(true);
        }
    }

    public void MigrationEnd() {
        foreach (MovingCar car in incomingCars) {
            car.SetMoving(false);
        }

        foreach (MovingCar car in outgoingCars) {
            car.SetMoving(false);
        }
    }

    public void SetSunOut(bool enabled)
    {
        if (enabled)
        {
            Lighting.GetComponent<Light>().intensity = 1.2f;
        } else
        {
            Lighting.GetComponent<Light>().intensity = 1.0f;
        }
        Sun.SetActive(enabled);
    }
}
