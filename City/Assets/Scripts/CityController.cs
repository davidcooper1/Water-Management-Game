using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour {

    public GameObject Lake;
    public GameObject RainParticles;
    public GameObject RainCloud;
    private float lakeMax;
    private float lakeCurrent;
    private bool lakeUpdateNeeded;

	// Use this for initialization
	void Start () {
        SetRainEnabled(false);
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

    public void SetRainEnabled(bool rainEnabled) {
        RainParticles.SetActive(rainEnabled);
        RainCloud.SetActive(rainEnabled);
    }
}
