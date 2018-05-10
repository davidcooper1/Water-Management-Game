using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingCar : MonoBehaviour {

    public Transform destination;
    private Vector3 startPosition;
    private bool keepMoving = true;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //agent.enabled = false;
        agent.destination = destination.position;
        gameObject.SetActive(false);
	}

    public void SetMoving(bool move) {
        if (!move) {
            GetComponent<NavMeshAgent>().enabled = false;
            gameObject.SetActive(false);
        } else {
            gameObject.SetActive(true);
            JumpBackToStart();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<NavMeshAgent>().destination = destination.position;
        }
    }

    void JumpBackToStart() {
        transform.position = startPosition;
    }
	
	// Update is called once per frame
	void Update () {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent.remainingDistance < 0.5f) {
            JumpBackToStart();
        }
	}
}
