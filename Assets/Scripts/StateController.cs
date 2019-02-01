using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {

    public State currentState;
    public State remainState;
    public FieldOfView eyes;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;

    private bool isAiActive;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("PatrolSpot");
        wayPointList = new List<Transform>();
        isAiActive = true;

        for (int i = 0; i < wayPoints.Length; i++)
            wayPointList.Add(wayPoints[i].transform);
    }

    void Update () {
        if (!isAiActive)
            return;

        currentState.UpdateState(this);
	}

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            //OnExitState(); Uncomment if needed later.
        }
    }

    private void OnExitState()
    {
        Debug.Log("Don't need yet!");
    }
}
