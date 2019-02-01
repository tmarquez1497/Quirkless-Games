using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {

    public State currentState;                                  // The current objective of the guard.
    public State remainState;                                   // Placeholder for an empty objective.
    public FieldOfView eyes;                                    // The script of whatever object forms the guard's line of sight.

    [HideInInspector] public NavMeshAgent navMeshAgent;         // The NavMeshAgent component
    [HideInInspector] public List<Transform> wayPointList;      // The list of points for the guard to patrol around (Set in Unity)
    [HideInInspector] public int nextWayPoint;                  // The number of the next point to patrol to
    [HideInInspector] public Transform chaseTarget;             // The transform of the object to chase (when chasing)

    private bool isAiActive;                                    // Is the navigation system on

    private void Awake()
    {
        // In here we grab the NavMeshAgent and store every patrol point (Taged with "PatrolSpot")
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("PatrolSpot");
        wayPointList = new List<Transform>();
        isAiActive = true;

        for (int i = 0; i < wayPoints.Length; i++)
            wayPointList.Add(wayPoints[i].transform);
    }

    void Update () {
        // If the navigation system is not on, do nothing.
        if (!isAiActive)
            return;

        // Otherwise, keep performing the current objective.
        currentState.UpdateState(this);
	}

    public void TransitionToState(State nextState)
    {
        // If the next objective is not empty, change to the new objective.
        if (nextState != remainState)
        {
            currentState = nextState;
            //OnExitState(); Uncomment if needed later.
        }
    }

    private void OnExitState()
    {
        // Placeholder for later features (if needed)
        Debug.Log("Don't need yet!");
    }
}
